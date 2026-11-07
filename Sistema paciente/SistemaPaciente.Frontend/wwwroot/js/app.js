import { renderCrud } from './crud.js';
import { renderDashboard } from './dashboard.js';
import { MODULES, moduleByKey } from './modules.js';
import { emptyState, h, icon } from './ui.js';

const content = document.getElementById('contenido');
const nav = document.getElementById('nav');
const sidebar = document.getElementById('sidebar');
const scrim = document.getElementById('scrim');
const menuBtn = document.getElementById('menuBtn');
const themeBtn = document.getElementById('themeBtn');
const topbarTitle = document.getElementById('topbarTitle');

// ---------- Menú lateral ----------
function buildNav() {
  const link = (key, title, iconName) => h('a', { class: 'nav-link', href: `#/${key}`, 'data-key': key }, icon(iconName), title);
  const grupos = [...new Set(MODULES.map(m => m.group))];

  nav.append(
    link('inicio', 'Inicio', 'home'),
    ...grupos.map(grupo => h('div', { class: 'nav-group' },
      h('div', { class: 'nav-title' }, grupo),
      MODULES.filter(m => m.group === grupo).map(m => link(m.key, m.title, m.icon)))),
  );
}

function setMenu(open) {
  sidebar.classList.toggle('open', open);
  scrim.classList.toggle('open', open);
  menuBtn.setAttribute('aria-expanded', String(open));
}

// ---------- Tema ----------
function currentTheme() {
  return document.documentElement.dataset.theme
    || (matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light');
}

function paintThemeButton() {
  const dark = currentTheme() === 'dark';
  themeBtn.replaceChildren(icon(dark ? 'sun' : 'moon'));
  themeBtn.setAttribute('aria-label', dark ? 'Usar tema claro' : 'Usar tema oscuro');
  themeBtn.title = themeBtn.getAttribute('aria-label');
}

themeBtn.addEventListener('click', () => {
  const siguiente = currentTheme() === 'dark' ? 'light' : 'dark';
  document.documentElement.dataset.theme = siguiente;
  try {
    localStorage.setItem('tema', siguiente);
  } catch { /* sin almacenamiento: el tema dura hasta recargar */ }
  paintThemeButton();
});

// ---------- Rutas ----------
function route() {
  const key = location.hash.replace(/^#\//, '') || 'inicio';
  const module = moduleByKey(key);
  const title = key === 'inicio' ? 'Inicio' : module?.title;

  document.title = title ? `${title} · Sistema de Pacientes` : 'Sistema de Pacientes';
  topbarTitle.textContent = title || '';
  nav.querySelectorAll('.nav-link').forEach(a => {
    if (a.dataset.key === key) a.setAttribute('aria-current', 'page');
    else a.removeAttribute('aria-current');
  });
  setMenu(false);

  if (key === 'inicio') renderDashboard(content);
  else if (module) renderCrud(content, module);
  else content.replaceChildren(h('div', { class: 'card' }, emptyState('search', 'Página no encontrada', 'Elige una opción del menú.')));

  window.scrollTo(0, 0);
}

menuBtn.append(icon('menu'));
menuBtn.addEventListener('click', () => setMenu(!sidebar.classList.contains('open')));
scrim.addEventListener('click', () => setMenu(false));
document.addEventListener('keydown', e => {
  if (e.key === 'Escape') setMenu(false);
});
window.addEventListener('hashchange', route);

buildNav();
paintThemeButton();
route();
