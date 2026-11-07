import { loadAll } from './api.js';
import { ageFrom, badge, emptyState, formatDateOnly, formatDateTime, fullName, h, openModal, toast } from './ui.js';

function dato(label, value) {
  return h('div', {}, h('dt', {}, label), h('dd', {}, value || '—'));
}

// Resumen clínico de un paciente: datos, alergias e historial de citas con su diagnóstico.
export async function openExpediente(paciente, { SANGRE, ESTADOS: estadoBadge, SEVERIDAD }) {
  let data;
  try {
    data = await loadAll(['alergias', 'citas', 'consultas', 'recetas', 'medicos', 'medicamentos', 'seguros']);
  } catch (error) {
    toast(error.message, 'error');
    return;
  }

  const seguro = data.seguros.find(s => s.id === paciente.seguroId);
  const alergias = data.alergias.filter(a => a.pacienteId === paciente.id);
  const citas = data.citas
    .filter(c => c.pacienteId === paciente.id)
    .sort((a, b) => b.fechaHoraInicio.localeCompare(a.fechaHoraInicio));

  const historial = citas.map(cita => {
    const medico = data.medicos.find(m => m.id === cita.medicoId);
    const consulta = data.consultas.find(c => c.citaId === cita.id);
    const recetas = consulta ? data.recetas.filter(r => r.consultaId === consulta.id) : [];
    return h('li', {},
      h('div', { class: 'timeline-head' },
        h('strong', {}, formatDateTime(cita.fechaHoraInicio)),
        estadoBadge(cita.estado)),
      h('div', { class: 'muted small' }, `${fullName(medico)} · ${cita.motivo}`),
      consulta ? h('div', {}, `Diagnóstico: ${consulta.diagnostico}`) : null,
      recetas.length
        ? h('div', { class: 'small muted' }, `Receta: ${recetas.map(r => data.medicamentos.find(m => m.id === r.medicamentoId)?.nombre).join(', ')}`)
        : null);
  });

  const modal = openModal({
    title: `Expediente de ${fullName(paciente)}`,
    body: [
      h('dl', { class: 'info-grid' },
        dato('Cédula', paciente.cedula),
        dato('Nacimiento', `${formatDateOnly(paciente.fechaNacimiento)} (${ageFrom(paciente.fechaNacimiento)} años)`),
        dato('Sexo', paciente.sexo),
        dato('Sangre', SANGRE[paciente.tipoSangre]),
        dato('Teléfono', paciente.telefono),
        dato('Correo', paciente.email),
        dato('Dirección', paciente.direccion),
        dato('Seguro', seguro ? `${seguro.nombre}${paciente.numeroAfiliado ? ` · ${paciente.numeroAfiliado}` : ''}` : 'Sin seguro')),
      h('h3', { class: 'section-title' }, 'Alergias'),
      alergias.length
        ? h('div', { class: 'chips' }, alergias.map(a => badge(`${a.sustancia} · ${a.severidad}`, SEVERIDAD[a.severidad].tone, SEVERIDAD[a.severidad].icon)))
        : h('p', { class: 'muted' }, 'Sin alergias registradas.'),
      h('h3', { class: 'section-title' }, `Historial de citas (${citas.length})`),
      citas.length
        ? h('ul', { class: 'timeline' }, historial)
        : emptyState('calendar', 'Sin citas todavía', 'Este paciente aún no tiene citas registradas.'),
    ],
    footer: [h('button', { class: 'btn', type: 'button', onclick: () => modal.close() }, 'Cerrar')],
  });
}
