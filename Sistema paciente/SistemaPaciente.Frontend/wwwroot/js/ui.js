// Crea elementos del DOM. El texto siempre entra como nodo de texto, nunca como HTML.
export function h(tag, attrs = {}, ...children) {
  const el = document.createElement(tag);
  for (const [key, value] of Object.entries(attrs || {})) {
    if (value === null || value === undefined || value === false) continue;
    if (key.startsWith('on')) el.addEventListener(key.slice(2), value);
    else if (key === 'class') el.className = value;
    else if (key === 'value') el.value = value;
    else if (value === true) el.setAttribute(key, '');
    else el.setAttribute(key, value);
  }
  el.append(...children.flat().filter(c => c !== null && c !== undefined && c !== false));
  return el;
}

const ICONS = {
  home: '<path d="M4 11.5 12 4l8 7.5"/><path d="M6 10v10h12V10"/><path d="M10 20v-6h4v6"/>',
  users: '<circle cx="9" cy="8" r="3.5"/><path d="M3 20c0-3.5 2.5-6 6-6s6 2.5 6 6"/><path d="M16 4.8a3.5 3.5 0 0 1 0 6.4"/><path d="M18 14.5c2 .8 3 2.8 3 5.5"/>',
  calendar: '<rect x="4" y="5.5" width="16" height="15" rx="2"/><path d="M4 10h16M8.5 3.5v4M15.5 3.5v4"/>',
  stethoscope: '<path d="M6 3.5H4.5V9a4.5 4.5 0 0 0 9 0V3.5H12"/><path d="M9 13.5V16a4.5 4.5 0 0 0 9 0v-2"/><circle cx="18" cy="11.5" r="2.2"/>',
  note: '<path d="M6 3.5h9l4 4v13H6z"/><path d="M15 3.5v4h4M9.5 12.5h6M9.5 16h6"/>',
  card: '<rect x="3" y="6" width="18" height="13" rx="2"/><path d="M3 10.5h18M7 15h3"/>',
  alert: '<path d="M12 4 2.8 20h18.4z"/><path d="M12 10v4.5M12 17.4v.1"/>',
  doctor: '<circle cx="12" cy="7.5" r="4"/><path d="M4.5 21c0-4.5 3-7.5 7.5-7.5s7.5 3 7.5 7.5"/><path d="M12 16v3.5M10.2 17.8h3.6"/>',
  clock: '<circle cx="12" cy="12" r="8.5"/><path d="M12 7.5V12l3 2"/>',
  tag: '<path d="M4 4h8l8 8-8 8-8-8z"/><circle cx="8.5" cy="8.5" r="1.2"/>',
  door: '<path d="M6 21V4.5A1.5 1.5 0 0 1 7.5 3h9A1.5 1.5 0 0 1 18 4.5V21"/><path d="M3.5 21h17M14.5 12v1"/>',
  pill: '<rect x="2.7" y="8" width="18.6" height="8" rx="4" transform="rotate(-45 12 12)"/><path d="m8.8 8.8 6.4 6.4"/>',
  shield: '<path d="M12 3.5 5 6v5.5c0 4.3 2.8 7.6 7 9 4.2-1.4 7-4.7 7-9V6z"/><path d="m9 12 2.2 2.2L15 10.4"/>',
  plus: '<path d="M12 5v14M5 12h14"/>',
  edit: '<path d="M4 20h4L19 9l-4-4L4 16z"/><path d="m13.5 6.5 4 4"/>',
  trash: '<path d="M4.5 7h15M9.5 7V4.5h5V7M6.5 7l1 13.5h9l1-13.5M10 11v5.5M14 11v5.5"/>',
  search: '<circle cx="11" cy="11" r="6.5"/><path d="m20 20-4.3-4.3"/>',
  close: '<path d="M6 6l12 12M18 6 6 18"/>',
  check: '<path d="m5 12.5 4.5 4.5L19 7.5"/>',
  chevronUp: '<path d="m6 15 6-6 6 6"/>',
  chevronDown: '<path d="m6 9 6 6 6-6"/>',
  menu: '<path d="M4 7h16M4 12h16M4 17h16"/>',
  sun: '<circle cx="12" cy="12" r="4"/><path d="M12 3v2M12 19v2M3 12h2M19 12h2M5.6 5.6 7 7M17 17l1.4 1.4M5.6 18.4 7 17M17 7l1.4-1.4"/>',
  moon: '<path d="M20 14.5A8.5 8.5 0 0 1 9.5 4a8.5 8.5 0 1 0 10.5 10.5z"/>',
  folder: '<path d="M3.5 18.5v-12A1.5 1.5 0 0 1 5 5h4.5l2 2.5H19a1.5 1.5 0 0 1 1.5 1.5v9.5A1.5 1.5 0 0 1 19 20H5a1.5 1.5 0 0 1-1.5-1.5z"/>',
  money: '<rect x="3" y="6.5" width="18" height="11" rx="2"/><circle cx="12" cy="12" r="2.5"/>',
  inbox: '<path d="M3.5 13.5 6 5h12l2.5 8.5V19h-17z"/><path d="M3.5 13.5h5l1.5 2.5h4l1.5-2.5h5"/>',
};

export function icon(name) {
  const svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
  svg.setAttribute('viewBox', '0 0 24 24');
  svg.setAttribute('class', 'icon');
  svg.setAttribute('aria-hidden', 'true');
  svg.innerHTML = ICONS[name] || '';
  return svg;
}

export function badge(text, tone = 'neutral', iconName) {
  return h('span', { class: `badge ${tone}` }, iconName ? icon(iconName) : null, text);
}

export function iconButton(iconName, label, onclick, tone = '') {
  return h('button', { class: `icon-btn ${tone}`, type: 'button', title: label, 'aria-label': label, onclick }, icon(iconName));
}

export function initials(person) {
  return `${person?.nombre?.[0] ?? ''}${person?.apellido?.[0] ?? ''}`.toUpperCase();
}

export function personCell(person, sub) {
  return h('div', { class: 'person' },
    h('span', { class: 'avatar', 'aria-hidden': 'true' }, initials(person)),
    h('div', {}, h('strong', {}, fullName(person)), sub ? h('span', { class: 'muted small' }, sub) : null));
}

export const fullName = person => (person ? `${person.nombre} ${person.apellido}` : '—');

// ---------- Formato ----------
const dinero = new Intl.NumberFormat('es-DO', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
const fechaCorta = new Intl.DateTimeFormat('es', { day: '2-digit', month: 'short', year: 'numeric' });
const hora = new Intl.DateTimeFormat('es', { hour: 'numeric', minute: '2-digit', hour12: true });

export const money = value => `$${dinero.format(value ?? 0)}`;
export const formatTime = value => hora.format(new Date(value));
export const formatDate = value => fechaCorta.format(new Date(value));
export const formatDateTime = value => `${formatDate(value)}, ${formatTime(value)}`;
// Una fecha sin hora ("1990-03-14") se lee como local para que no cambie de día.
export const formatDateOnly = value => (value ? fechaCorta.format(new Date(`${value}T00:00:00`)) : '—');
export const formatHour = value => (value ? formatTime(`2000-01-01T${value}`) : '—');
export const today = () => new Date().toLocaleDateString('en-CA');

export function ageFrom(dateOnly) {
  const nacimiento = new Date(`${dateOnly}T00:00:00`);
  const ahora = new Date();
  let edad = ahora.getFullYear() - nacimiento.getFullYear();
  const mes = ahora.getMonth() - nacimiento.getMonth();
  if (mes < 0 || (mes === 0 && ahora.getDate() < nacimiento.getDate())) edad--;
  return edad;
}

// ---------- Avisos ----------
export function toast(message, type = 'ok') {
  const el = h('div', { class: `toast ${type === 'error' ? 'error' : ''}` },
    icon(type === 'error' ? 'alert' : 'check'),
    h('span', {}, message));
  document.getElementById('toasts').append(el);
  setTimeout(() => el.remove(), type === 'error' ? 6500 : 3500);
}

// ---------- Modales ----------
export function openModal({ title, body, footer, narrow = false, form = null }) {
  const dialog = h('dialog', { class: `modal ${narrow ? 'narrow' : ''}`, 'aria-label': title });
  const close = () => dialog.close();
  const inner = form || h('div', { class: 'modal-form' });
  inner.append(
    h('div', { class: 'modal-head' }, h('h2', {}, title), iconButton('close', 'Cerrar', close)),
    h('div', { class: 'modal-body' }, body),
    footer ? h('div', { class: 'modal-foot' }, footer) : null,
  );
  dialog.append(inner);
  dialog.addEventListener('close', () => dialog.remove());
  document.body.append(dialog);
  dialog.showModal();
  return { dialog, close };
}

export function confirmDialog({ title, message, confirmLabel = 'Confirmar', danger = true }) {
  return new Promise(resolve => {
    let accepted = false;
    const confirmBtn = h('button', {
      class: `btn ${danger ? 'btn-danger' : 'btn-primary'}`,
      type: 'button',
      onclick: () => { accepted = true; modal.close(); },
    }, confirmLabel);
    const modal = openModal({
      title,
      narrow: true,
      body: h('p', {}, message),
      footer: [h('button', { class: 'btn', type: 'button', onclick: () => modal.close() }, 'Volver'), confirmBtn],
    });
    modal.dialog.addEventListener('close', () => resolve(accepted));
  });
}

// ---------- Estados de página ----------
export function skeleton() {
  return h('div', { class: 'skeleton', 'aria-label': 'Cargando', role: 'status' }, Array.from({ length: 7 }, () => h('div')));
}

export function emptyState(iconName, title, text, action) {
  return h('div', { class: 'state' }, icon(iconName), h('h2', {}, title), text ? h('p', {}, text) : null, action);
}

export function errorState(message, retry) {
  return emptyState('alert', 'No se pudieron cargar los datos', message,
    h('button', { class: 'btn', type: 'button', onclick: retry }, 'Reintentar'));
}
