import { loadAll } from './api.js';
import { ESTADOS, estadoBadge } from './modules.js';
import { emptyState, errorState, formatDate, formatTime, fullName, h, icon, money, skeleton, today } from './ui.js';

function statTile(iconName, label, value, note) {
  return h('article', { class: 'card stat' },
    h('div', { class: 'stat-label' }, icon(iconName), label),
    h('div', { class: 'stat-value' }, value),
    note ? h('div', { class: 'stat-note' }, note) : null);
}

export function renderDashboard(root) {
  async function load() {
    root.replaceChildren(h('div', { class: 'card' }, skeleton()));

    try {
      const { citas, pacientes, medicos, pagos } = await loadAll(['citas', 'pacientes', 'medicos', 'pagos']);
      const hoy = today();
      const ahora = new Date();
      const activas = c => c.estado !== 'Cancelada';

      const deHoy = citas.filter(c => c.fechaHoraInicio.startsWith(hoy) && activas(c));
      const porConfirmar = citas.filter(c => c.estado === 'Pendiente').length;
      const mes = hoy.slice(0, 7);
      const cobradoMes = pagos.filter(p => p.fechaPago.startsWith(mes)).reduce((suma, p) => suma + p.monto, 0);
      const proximas = citas
        .filter(c => (c.estado === 'Pendiente' || c.estado === 'Confirmada') && new Date(c.fechaHoraInicio) >= ahora)
        .sort((a, b) => a.fechaHoraInicio.localeCompare(b.fechaHoraInicio))
        .slice(0, 6);

      const nombreDe = (lista, id) => fullName(lista.find(x => x.id === id));
      const maximo = Math.max(1, ...Object.keys(ESTADOS).map(e => citas.filter(c => c.estado === e).length));

      root.replaceChildren(
        h('div', { class: 'page-head' }, h('div', {},
          h('h1', {}, 'Inicio'),
          h('p', {}, new Intl.DateTimeFormat('es', { dateStyle: 'full' }).format(ahora)))),
        h('section', { class: 'stats', 'aria-label': 'Resumen' },
          statTile('users', 'Pacientes', String(pacientes.length), 'registrados'),
          statTile('calendar', 'Citas de hoy', String(deHoy.length), 'sin contar canceladas'),
          statTile('clock', 'Por confirmar', String(porConfirmar), 'citas pendientes'),
          statTile('money', 'Cobrado este mes', money(cobradoMes), `${pagos.filter(p => p.fechaPago.startsWith(mes)).length} pago(s)`)),
        h('div', { class: 'dash-grid' },
          h('section', { class: 'card' },
            h('div', { class: 'card-head' }, h('h2', {}, 'Próximas citas'), h('a', { href: '#/citas' }, 'Ver todas')),
            proximas.length
              ? h('ul', { class: 'agenda' }, proximas.map(c => h('li', {},
                h('div', { class: 'agenda-time' },
                  h('strong', {}, formatTime(c.fechaHoraInicio)),
                  h('span', { class: 'muted small' }, formatDate(c.fechaHoraInicio))),
                h('div', { class: 'agenda-main' },
                  h('strong', {}, nombreDe(pacientes, c.pacienteId)),
                  h('span', { class: 'muted small' }, `${nombreDe(medicos, c.medicoId)} · ${c.motivo}`)),
                estadoBadge(c.estado))))
              : emptyState('calendar', 'No hay citas próximas', 'Las citas pendientes y confirmadas aparecerán aquí.')),
          h('section', { class: 'card' },
            h('div', { class: 'card-head' }, h('h2', {}, 'Citas por estado')),
            h('div', { class: 'bars' }, Object.keys(ESTADOS).map(estado => {
              const total = citas.filter(c => c.estado === estado).length;
              return h('div', { title: `${estado}: ${total}` },
                h('div', { class: 'bar-head' }, h('span', {}, estado), h('strong', {}, String(total))),
                h('div', { class: 'bar-track' },
                  total ? h('div', { class: 'bar-fill', style: `width:${(total / maximo) * 100}%` }) : null));
            })))),
      );
    } catch (error) {
      root.replaceChildren(h('div', { class: 'card' }, errorState(error.message, load)));
    }
  }

  load();
}
