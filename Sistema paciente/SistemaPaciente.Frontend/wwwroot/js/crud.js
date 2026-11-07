import { api, loadAll } from './api.js';
import { openForm } from './form.js';
import { confirmDialog, emptyState, errorState, h, icon, iconButton, skeleton, toast } from './ui.js';

const POR_PAGINA = 10;
const textOf = value => (value === null || value === undefined ? '' : String(value));

// Página genérica de CRUD: lista, búsqueda, filtros, orden, paginación, crear, editar y eliminar.
export function renderCrud(root, module) {
  const state = { rows: [], ctx: {}, query: '', filters: {}, sort: null, page: 1 };

  const body = h('div', {}, skeleton());
  const newBtn = h('button', {
    class: 'btn btn-primary',
    type: 'button',
    onclick: () => openForm(module, { onSaved: load }),
  }, icon('plus'), module.newLabel);
  const subtitle = h('p', {}, module.description);

  root.replaceChildren(
    h('div', { class: 'page-head' }, h('div', {}, h('h1', {}, module.title), subtitle), newBtn),
    h('section', { class: 'card', 'aria-label': module.title }, body),
  );

  async function load() {
    try {
      const data = await loadAll([module.key, ...(module.refs || [])]);
      state.rows = data[module.key];
      state.ctx = data;
      renderShell();
    } catch (error) {
      body.replaceChildren(errorState(error.message, () => {
        body.replaceChildren(skeleton());
        load();
      }));
    }
  }

  const tableHost = h('div');

  function renderShell() {
    if (state.rows.length === 0) {
      body.replaceChildren(emptyState(module.icon, `Aún no hay registros de ${module.title.toLowerCase()}`, module.emptyText));
      return;
    }

    const search = h('div', { class: 'search' },
      icon('search'),
      h('input', {
        type: 'search',
        placeholder: module.searchPlaceholder || 'Buscar…',
        'aria-label': `Buscar en ${module.title}`,
        value: state.query,
        oninput: e => {
          state.query = e.target.value;
          state.page = 1;
          renderTable();
        },
      }));

    const filters = (module.filters || []).map(filter => {
      const select = h('select', {
        'aria-label': filter.label,
        onchange: e => {
          state.filters[filter.name] = e.target.value;
          state.page = 1;
          renderTable();
        },
      },
        h('option', { value: '' }, filter.label),
        filter.options.map(o => h('option', { value: o.value }, o.label)));
      select.value = state.filters[filter.name] || '';
      return select;
    });

    body.replaceChildren(h('div', { class: 'toolbar' }, search, filters), tableHost);
    renderTable();
  }

  function visibleRows() {
    const q = state.query.trim().toLowerCase();
    let rows = state.rows.filter(row => {
      const pasaFiltros = (module.filters || []).every(f => !state.filters[f.name] || f.value(row) === state.filters[f.name]);
      if (!pasaFiltros) return false;
      if (!q) return true;
      return module.columns.some(col => textOf(col.value(row, state.ctx)).toLowerCase().includes(q));
    });

    if (state.sort) {
      const { col, dir } = state.sort;
      rows = [...rows].sort((a, b) => {
        const va = col.value(a, state.ctx);
        const vb = col.value(b, state.ctx);
        const cmp = typeof va === 'number' && typeof vb === 'number'
          ? va - vb
          : textOf(va).localeCompare(textOf(vb), 'es', { numeric: true });
        return cmp * dir;
      });
    } else if (module.initialSort) {
      rows = [...rows].sort(module.initialSort);
    }
    return rows;
  }

  function renderTable() {
    const rows = visibleRows();
    const totalPaginas = Math.max(1, Math.ceil(rows.length / POR_PAGINA));
    state.page = Math.min(state.page, totalPaginas);
    const inicio = (state.page - 1) * POR_PAGINA;
    const pagina = rows.slice(inicio, inicio + POR_PAGINA);

    const headers = module.columns.map(col => {
      const activo = state.sort?.col === col;
      const ascendente = activo && state.sort.dir === 1;
      return h('th', {
        class: col.num ? 'num' : '',
        scope: 'col',
        'aria-sort': activo ? (ascendente ? 'ascending' : 'descending') : null,
      }, h('button', {
        class: 'sort-btn',
        type: 'button',
        onclick: () => {
          state.sort = { col, dir: ascendente ? -1 : 1 };
          renderTable();
        },
      }, col.label, icon(activo && !ascendente ? 'chevronDown' : 'chevronUp')));
    });

    const filas = pagina.map(row => h('tr', {},
      module.columns.map(col => h('td', { class: col.num ? 'num' : '' }, col.render ? col.render(row, state.ctx) : (textOf(col.value(row, state.ctx)) || '—'))),
      h('td', {}, h('div', { class: 'actions' }, rowActions(row)))));

    if (rows.length === 0) {
      tableHost.replaceChildren(emptyState('search', 'Sin resultados', 'Ninguna fila coincide con la búsqueda o el filtro.'));
      return;
    }

    tableHost.replaceChildren(
      h('div', { class: 'table-wrap' },
        h('table', {},
          h('thead', {}, h('tr', {}, headers, h('th', { scope: 'col' }, h('span', { class: 'sr-hidden' }, 'Acciones')))),
          h('tbody', {}, filas))),
      h('div', { class: 'pager' },
        h('span', {}, `${inicio + 1}–${inicio + pagina.length} de ${rows.length}`),
        h('div', { class: 'pager-btns' },
          h('button', { class: 'btn', type: 'button', disabled: state.page === 1, onclick: () => { state.page--; renderTable(); } }, 'Anterior'),
          h('span', {}, `Página ${state.page} de ${totalPaginas}`),
          h('button', { class: 'btn', type: 'button', disabled: state.page === totalPaginas, onclick: () => { state.page++; renderTable(); } }, 'Siguiente'))));
  }

  function rowActions(row) {
    const custom = module.actions ? module.actions(row, state.ctx, { reload: load }) : [];
    const buttons = custom.map(a => iconButton(a.icon, a.label, a.onClick, a.tone));

    if (!module.canEdit || module.canEdit(row)) {
      buttons.push(iconButton('edit', `Editar ${module.singular}`, () => openForm(module, { row, onSaved: load })));
    }

    buttons.push(iconButton('trash', `Eliminar ${module.singular}`, async () => {
      const ok = await confirmDialog({
        title: `Eliminar ${module.singular}`,
        message: `Se eliminará: ${module.describe(row, state.ctx)}. Esta acción no se puede deshacer.`,
        confirmLabel: 'Eliminar',
      });
      if (!ok) return;
      try {
        await api.remove(module.key, row.id);
        toast('Registro eliminado.');
        load();
      } catch (error) {
        toast(error.message, 'error');
      }
    }, 'danger'));

    return buttons;
  }

  load();
}
