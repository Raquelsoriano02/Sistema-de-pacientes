import { api, loadAll } from './api.js';
import { formatTime, h, icon, openModal, toast, today } from './ui.js';

// Cada constructor devuelve el elemento y una función read() con el valor a enviar a la API.
function buildSelect(field, value, ctx, row) {
  const options = typeof field.options === 'function' ? field.options(ctx, row) : field.options;
  const select = h('select', {
    id: `f-${field.name}`,
    name: field.name,
    required: field.required,
    disabled: row && field.lockOnEdit,
  },
    h('option', { value: '' }, field.required ? 'Selecciona…' : (field.emptyLabel || 'Ninguno')),
    options.map(o => h('option', { value: o.value }, o.label)));
  select.value = value ?? '';
  return {
    control: select,
    read: () => {
      if (select.value === '') return null;
      return field.numeric ? Number(select.value) : select.value;
    },
  };
}

function buildInput(field, value) {
  const isTextarea = field.type === 'textarea';
  let initial = value ?? '';
  if (field.type === 'time' && value) initial = value.slice(0, 5);

  const input = h(isTextarea ? 'textarea' : 'input', {
    id: `f-${field.name}`,
    name: field.name,
    type: isTextarea ? null : (field.type || 'text'),
    required: field.required,
    maxlength: field.max,
    min: field.min,
    max: field.type === 'number' || field.type === 'date' ? field.maxValue : null,
    step: field.step,
    placeholder: field.placeholder,
    autocomplete: 'off',
    rows: isTextarea ? 3 : null,
    value: initial,
  });

  return {
    control: input,
    read: () => {
      const v = input.value.trim();
      if (v === '') return null;
      if (field.type === 'number') return Number(v);
      if (field.type === 'time') return `${v}:00`;
      return v;
    },
  };
}

function buildCheckbox(field, value) {
  const input = h('input', { type: 'checkbox', name: field.name, id: `f-${field.name}`, checked: value ?? field.default ?? true });
  return {
    control: h('label', { class: 'check' }, input, field.checkLabel || 'Sí'),
    read: () => input.checked,
  };
}

// Selector de horario de la cita: fecha + horas libres devueltas por la API.
function buildSlots(field, value, formEl, row) {
  const original = value || null;
  let selected = original;

  const date = h('input', {
    id: `f-${field.name}`,
    type: 'date',
    min: today(),
    required: true,
    value: original ? original.slice(0, 10) : '',
  });
  const slots = h('div', { class: 'slots', role: 'group', 'aria-label': 'Horas disponibles' });
  const hint = h('p', { class: 'hint' });

  function renderSlots(lista) {
    slots.replaceChildren(...lista.map(iso => h('button', {
      class: 'slot',
      type: 'button',
      'aria-pressed': String(iso === selected),
      onclick: () => {
        selected = iso;
        renderSlots(lista);
      },
    }, formatTime(iso))));
  }

  let peticion = 0;
  async function load() {
    const medicoId = formEl.elements.medicoId.value;
    const consultorioId = formEl.elements.consultorioId.value;
    const fecha = date.value;
    const id = ++peticion;

    slots.replaceChildren();
    if (selected && selected.slice(0, 10) !== fecha) selected = null;

    if (!medicoId || !consultorioId || !fecha) {
      hint.textContent = 'Elige médico, consultorio y fecha para ver las horas libres.';
      return;
    }

    hint.textContent = 'Buscando horas libres…';
    try {
      const libres = await api.disponibilidad(medicoId, consultorioId, fecha);
      if (id !== peticion) return;

      // Al editar, la hora actual de la cita sigue siendo válida aunque figure como ocupada.
      const mismoLugar = row && String(row.medicoId) === medicoId && String(row.consultorioId) === consultorioId;
      if (original && mismoLugar && original.slice(0, 10) === fecha && !libres.includes(original)) {
        libres.push(original);
        libres.sort();
      }

      if (selected && !libres.includes(selected)) selected = null;
      hint.textContent = libres.length ? '' : 'No hay horas libres ese día. Prueba otra fecha.';
      renderSlots(libres);
    } catch (error) {
      if (id === peticion) hint.textContent = error.message;
    }
  }

  date.addEventListener('change', load);
  formEl.addEventListener('change', e => {
    if (e.target.name === 'medicoId' || e.target.name === 'consultorioId') load();
  });
  queueMicrotask(load);

  return {
    control: h('div', {}, date, slots, hint),
    read: () => selected,
    validate: () => (selected ? null : 'Selecciona una hora para la cita.'),
  };
}

export async function openForm(module, { row = null, prefill = null, onSaved } = {}) {
  let ctx;
  try {
    ctx = await loadAll([...(module.refs || []), module.key]);
  } catch (error) {
    toast(error.message, 'error');
    return;
  }

  const values = row || prefill || {};
  const formEl = h('form', { class: 'modal-form' });
  const errorBox = h('div', { class: 'form-error', role: 'alert', hidden: true });
  const readers = [];

  const grid = h('div', { class: 'form-grid' }, module.fields.map(field => {
    let built;
    if (field.type === 'select') built = buildSelect(field, values[field.name], ctx, row);
    else if (field.type === 'checkbox') built = buildCheckbox(field, values[field.name]);
    else if (field.type === 'slots') built = buildSlots(field, values[field.name], formEl, row);
    else built = buildInput(field, values[field.name]);

    readers.push({ field, ...built });

    return h('div', { class: `field ${field.span === 2 || field.type === 'slots' ? 'span-2' : ''}` },
      h('label', { for: `f-${field.name}` }, field.label, field.required ? h('span', { class: 'req', 'aria-hidden': 'true' }, '*') : null),
      built.control,
      field.hint ? h('p', { class: 'hint' }, field.hint) : null);
  }));

  const submit = h('button', { class: 'btn btn-primary', type: 'submit' }, row ? 'Guardar cambios' : `Crear ${module.singular}`);
  const modal = openModal({
    title: row ? `Editar ${module.singular}` : module.newLabel,
    form: formEl,
    body: [errorBox, grid],
    footer: [h('button', { class: 'btn', type: 'button', onclick: () => modal.close() }, 'Cancelar'), submit],
  });

  function showError(message) {
    errorBox.replaceChildren(icon('alert'), h('span', {}, message));
    errorBox.hidden = false;
    errorBox.scrollIntoView({ block: 'nearest' });
  }

  formEl.addEventListener('submit', async event => {
    event.preventDefault();
    errorBox.hidden = true;

    for (const { validate } of readers) {
      const problema = validate?.();
      if (problema) return showError(problema);
    }

    const data = Object.fromEntries(readers.map(({ field, read }) => [field.name, read()]));

    submit.disabled = true;
    try {
      if (row) await api.update(module.key, row.id, data);
      else await api.create(module.key, data);
      modal.close();
      toast(row ? 'Cambios guardados.' : `Registro de ${module.singular} creado.`);
      onSaved?.();
    } catch (error) {
      showError(error.message);
      submit.disabled = false;
    }
  });
}
