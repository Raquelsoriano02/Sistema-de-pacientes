import { api } from './api.js';
import { openForm } from './form.js';
import { openExpediente } from './expediente.js';
import {
  ageFrom, badge, confirmDialog, formatDate, formatDateTime, formatHour, formatTime,
  fullName, h, money, personCell, toast, today,
} from './ui.js';

// ---------- Catálogos de los enums de la API ----------
const toOptions = labels => Object.entries(labels).map(([value, label]) => ({ value, label }));

const DIAS = { Lunes: 'Lunes', Martes: 'Martes', Miercoles: 'Miércoles', Jueves: 'Jueves', Viernes: 'Viernes', Sabado: 'Sábado', Domingo: 'Domingo' };
const ORDEN_DIAS = Object.keys(DIAS);
const SEXOS = { Femenino: 'Femenino', Masculino: 'Masculino', Otro: 'Otro' };
const SANGRE = { APositivo: 'A+', ANegativo: 'A−', BPositivo: 'B+', BNegativo: 'B−', ABPositivo: 'AB+', ABNegativo: 'AB−', OPositivo: 'O+', ONegativo: 'O−' };
const METODOS = { Efectivo: 'Efectivo', Tarjeta: 'Tarjeta', Transferencia: 'Transferencia' };
const SEVERIDAD = {
  Leve: { tone: 'info', icon: 'alert' },
  Moderada: { tone: 'warn', icon: 'alert' },
  Severa: { tone: 'danger', icon: 'alert' },
};
export const ESTADOS = {
  Pendiente: { tone: 'warn', icon: 'clock' },
  Confirmada: { tone: 'info', icon: 'calendar' },
  Completada: { tone: 'good', icon: 'check' },
  Cancelada: { tone: 'danger', icon: 'close' },
};

export const estadoBadge = estado => badge(estado, ESTADOS[estado].tone, ESTADOS[estado].icon);
const activoBadge = activo => (activo ? badge('Activo', 'good', 'check') : badge('Inactivo', 'neutral', 'close'));
const activaBadge = activa => (activa ? badge('Activa', 'good', 'check') : badge('Inactiva', 'neutral', 'close'));

// ---------- Ayudas para relacionar datos ----------
const byId = (list, id) => list?.find(item => item.id === id);
const pacienteDe = (ctx, cita) => byId(ctx.pacientes, cita?.pacienteId);
const citaLabel = (ctx, cita) => (cita ? `${fullName(pacienteDe(ctx, cita))} · ${formatDateTime(cita.fechaHoraInicio)}` : '—');
const consultaLabel = (ctx, consulta) => {
  if (!consulta) return '—';
  return `${fullName(pacienteDe(ctx, byId(ctx.citas, consulta.citaId)))} · ${consulta.diagnostico}`;
};
const totalCita = cita => cita.costo - cita.cobertura;
const pagadoCita = (ctx, citaId, excepto) => (ctx.pagos || [])
  .filter(p => p.citaId === citaId && p.id !== excepto)
  .reduce((suma, p) => suma + p.monto, 0);
const saldoCita = (ctx, cita, excepto) => totalCita(cita) - pagadoCita(ctx, cita.id, excepto);

// En los selectores se ocultan los registros inactivos, salvo el que ya tiene el registro editado.
const activeOptions = (list, flag, current, label) => list
  .filter(item => item[flag] !== false || item.id === current)
  .map(item => ({ value: item.id, label: label(item) }));

const emailField = { name: 'email', label: 'Correo', type: 'email', max: 150, placeholder: 'nombre@correo.com' };
const nameFields = [
  { name: 'nombre', label: 'Nombre', required: true, max: 100 },
  { name: 'apellido', label: 'Apellido', required: true, max: 100 },
];

// ---------- Módulos ----------
export const MODULES = [
  {
    key: 'citas', title: 'Citas', singular: 'cita', newLabel: 'Nueva cita', icon: 'calendar', group: 'Atención',
    description: 'Agenda, confirmación y cobro de citas.',
    emptyText: 'Crea la primera cita para empezar a llenar la agenda.',
    searchPlaceholder: 'Buscar por paciente, médico o motivo…',
    refs: ['pacientes', 'medicos', 'consultorios', 'pagos'],
    initialSort: (a, b) => b.fechaHoraInicio.localeCompare(a.fechaHoraInicio),
    canEdit: row => row.estado === 'Pendiente' || row.estado === 'Confirmada',
    filters: [{ name: 'estado', label: 'Todos los estados', value: r => r.estado, options: toOptions(Object.fromEntries(Object.keys(ESTADOS).map(e => [e, e]))) }],
    describe: (r, c) => `la cita de ${citaLabel(c, r)}`,
    columns: [
      { label: 'Fecha', value: r => r.fechaHoraInicio, render: r => h('div', { class: 'nowrap' }, h('strong', {}, formatDate(r.fechaHoraInicio)), h('div', { class: 'muted small' }, formatTime(r.fechaHoraInicio))) },
      { label: 'Paciente', value: (r, c) => fullName(byId(c.pacientes, r.pacienteId)), render: (r, c) => personCell(byId(c.pacientes, r.pacienteId)) },
      { label: 'Médico', value: (r, c) => fullName(byId(c.medicos, r.medicoId)) },
      { label: 'Consultorio', value: (r, c) => byId(c.consultorios, r.consultorioId)?.codigo },
      { label: 'Motivo', value: r => r.motivo, render: r => h('span', { class: 'cell-clip', title: r.motivo }, r.motivo) },
      { label: 'Estado', value: r => r.estado, render: r => estadoBadge(r.estado) },
      { label: 'A pagar', num: true, value: r => totalCita(r), render: r => money(totalCita(r)) },
      { label: 'Saldo', num: true, value: (r, c) => saldoCita(c, r), render: (r, c) => money(saldoCita(c, r)) },
    ],
    fields: [
      { name: 'pacienteId', label: 'Paciente', type: 'select', numeric: true, required: true, span: 2, options: (c, r) => c.pacientes.map(p => ({ value: p.id, label: `${fullName(p)} · ${p.cedula}` })) },
      { name: 'medicoId', label: 'Médico', type: 'select', numeric: true, required: true, options: (c, r) => activeOptions(c.medicos, 'activo', r?.medicoId, fullName) },
      { name: 'consultorioId', label: 'Consultorio', type: 'select', numeric: true, required: true, options: (c, r) => activeOptions(c.consultorios, 'activo', r?.consultorioId, x => x.codigo) },
      { name: 'fechaHoraInicio', label: 'Fecha y hora', type: 'slots', required: true },
      { name: 'motivo', label: 'Motivo de la cita', type: 'textarea', required: true, max: 500, span: 2 },
    ],
    actions: (row, ctx, { reload }) => {
      const acciones = [];
      const abierta = row.estado === 'Pendiente' || row.estado === 'Confirmada';
      if (row.estado === 'Pendiente') {
        acciones.push({
          icon: 'check', label: 'Confirmar cita', tone: 'good',
          onClick: () => cambiarEstado(row, 'Confirmada', reload),
        });
      }
      if (abierta) {
        acciones.push({
          icon: 'stethoscope', label: 'Registrar consulta',
          onClick: () => openForm(moduleByKey('consultas'), { prefill: { citaId: row.id }, onSaved: reload }),
        });
      }
      if (row.estado !== 'Cancelada' && saldoCita(ctx, row) > 0) {
        acciones.push({
          icon: 'money', label: 'Registrar pago',
          onClick: () => openForm(moduleByKey('pagos'), { prefill: { citaId: row.id, monto: saldoCita(ctx, row) }, onSaved: reload }),
        });
      }
      if (abierta) {
        acciones.push({
          icon: 'close', label: 'Cancelar cita', tone: 'danger',
          onClick: async () => {
            const ok = await confirmDialog({
              title: 'Cancelar cita',
              message: `Se cancelará la cita de ${citaLabel(ctx, row)}. Una cita cancelada no se puede reactivar.`,
              confirmLabel: 'Cancelar cita',
            });
            if (ok) cambiarEstado(row, 'Cancelada', reload);
          },
        });
      }
      return acciones;
    },
  },
  {
    key: 'consultas', title: 'Consultas', singular: 'consulta', newLabel: 'Nueva consulta', icon: 'stethoscope', group: 'Atención',
    description: 'Signos vitales y diagnóstico. Registrar una consulta completa la cita.',
    emptyText: 'Registra una consulta desde esta página o desde una cita.',
    searchPlaceholder: 'Buscar por paciente o diagnóstico…',
    refs: ['citas', 'pacientes', 'medicos'],
    initialSort: (a, b) => b.id - a.id,
    describe: (r, c) => `la consulta de ${consultaLabel(c, r)}`,
    columns: [
      { label: 'Paciente', value: (r, c) => fullName(pacienteDe(c, byId(c.citas, r.citaId))), render: (r, c) => personCell(pacienteDe(c, byId(c.citas, r.citaId))) },
      { label: 'Médico', value: (r, c) => fullName(byId(c.medicos, byId(c.citas, r.citaId)?.medicoId)) },
      { label: 'Fecha', value: (r, c) => byId(c.citas, r.citaId)?.fechaHoraInicio, render: (r, c) => h('span', { class: 'nowrap' }, formatDate(byId(c.citas, r.citaId).fechaHoraInicio)) },
      { label: 'Diagnóstico', value: r => r.diagnostico, render: r => h('span', { class: 'cell-clip', title: r.diagnostico }, r.diagnostico) },
      { label: 'Presión', value: r => r.presionArterial },
      { label: 'Temp.', num: true, value: r => r.temperaturaC, render: r => (r.temperaturaC != null ? `${r.temperaturaC} °C` : '—') },
      { label: 'Peso', num: true, value: r => r.pesoKg, render: r => (r.pesoKg != null ? `${r.pesoKg} kg` : '—') },
    ],
    fields: [
      {
        name: 'citaId', label: 'Cita', type: 'select', numeric: true, required: true, span: 2, lockOnEdit: true,
        hint: 'Solo aparecen las citas pendientes o confirmadas.',
        options: (c, r) => c.citas
          .filter(x => x.estado === 'Pendiente' || x.estado === 'Confirmada' || x.id === r?.citaId)
          .map(x => ({ value: x.id, label: citaLabel(c, x) })),
      },
      { name: 'pesoKg', label: 'Peso (kg)', type: 'number', min: 0.1, maxValue: 500, step: 0.01 },
      { name: 'temperaturaC', label: 'Temperatura (°C)', type: 'number', min: 25, maxValue: 45, step: 0.01 },
      { name: 'presionArterial', label: 'Presión arterial', max: 20, placeholder: '120/80' },
      { name: 'sintomas', label: 'Síntomas', type: 'textarea', max: 1000, span: 2 },
      { name: 'diagnostico', label: 'Diagnóstico', type: 'textarea', required: true, max: 1000, span: 2 },
      { name: 'tratamiento', label: 'Tratamiento', type: 'textarea', max: 1000, span: 2 },
    ],
  },
  {
    key: 'recetas', title: 'Recetas', singular: 'receta', newLabel: 'Nueva receta', icon: 'note', group: 'Atención',
    description: 'Medicamentos indicados en cada consulta. Se bloquean los que coinciden con una alergia registrada.',
    emptyText: 'Las recetas se crean a partir de una consulta registrada.',
    searchPlaceholder: 'Buscar por paciente o medicamento…',
    refs: ['consultas', 'citas', 'pacientes', 'medicamentos'],
    initialSort: (a, b) => b.id - a.id,
    describe: (r, c) => `la receta de ${byId(c.medicamentos, r.medicamentoId)?.nombre}`,
    columns: [
      { label: 'Paciente', value: (r, c) => fullName(pacienteDe(c, byId(c.citas, byId(c.consultas, r.consultaId)?.citaId))), render: (r, c) => personCell(pacienteDe(c, byId(c.citas, byId(c.consultas, r.consultaId)?.citaId))) },
      { label: 'Medicamento', value: (r, c) => byId(c.medicamentos, r.medicamentoId)?.nombre },
      { label: 'Dosis', value: r => r.dosis },
      { label: 'Frecuencia', value: r => r.frecuencia },
      { label: 'Duración', num: true, value: r => r.duracionDias, render: r => `${r.duracionDias} día${r.duracionDias === 1 ? '' : 's'}` },
      { label: 'Indicaciones', value: r => r.indicaciones, render: r => h('span', { class: 'cell-clip', title: r.indicaciones || '' }, r.indicaciones || '—') },
    ],
    fields: [
      { name: 'consultaId', label: 'Consulta', type: 'select', numeric: true, required: true, span: 2, options: c => c.consultas.map(x => ({ value: x.id, label: consultaLabel(c, x) })) },
      { name: 'medicamentoId', label: 'Medicamento', type: 'select', numeric: true, required: true, span: 2, options: (c, r) => activeOptions(c.medicamentos, 'activo', r?.medicamentoId, m => m.nombre) },
      { name: 'dosis', label: 'Dosis', required: true, max: 100, placeholder: '1 tableta' },
      { name: 'frecuencia', label: 'Frecuencia', required: true, max: 100, placeholder: 'Cada 8 horas' },
      { name: 'duracionDias', label: 'Duración (días)', type: 'number', required: true, min: 1, maxValue: 365, step: 1 },
      { name: 'indicaciones', label: 'Indicaciones', type: 'textarea', max: 500, span: 2 },
    ],
  },
  {
    key: 'pagos', title: 'Pagos', singular: 'pago', newLabel: 'Registrar pago', icon: 'card', group: 'Atención',
    description: 'Cobros de citas. Un pago nunca puede superar el saldo pendiente.',
    emptyText: 'Registra un pago desde esta página o desde una cita.',
    searchPlaceholder: 'Buscar por paciente o referencia…',
    refs: ['citas', 'pacientes'],
    initialSort: (a, b) => b.fechaPago.localeCompare(a.fechaPago),
    filters: [{ name: 'metodoPago', label: 'Todos los métodos', value: r => r.metodoPago, options: toOptions(METODOS) }],
    describe: (r, c) => `el pago de ${money(r.monto)} de ${fullName(pacienteDe(c, byId(c.citas, r.citaId)))}`,
    columns: [
      { label: 'Paciente', value: (r, c) => fullName(pacienteDe(c, byId(c.citas, r.citaId))), render: (r, c) => personCell(pacienteDe(c, byId(c.citas, r.citaId))) },
      { label: 'Fecha de pago', value: r => r.fechaPago, render: r => h('span', { class: 'nowrap' }, formatDateTime(r.fechaPago)) },
      { label: 'Método', value: r => r.metodoPago },
      { label: 'Referencia', value: r => r.referencia },
      { label: 'Monto', num: true, value: r => r.monto, render: r => money(r.monto) },
    ],
    fields: [
      {
        name: 'citaId', label: 'Cita', type: 'select', numeric: true, required: true, span: 2,
        options: (c, r) => c.citas
          .filter(x => x.estado !== 'Cancelada' && (saldoCita(c, x, r?.id) > 0 || x.id === r?.citaId))
          .map(x => ({ value: x.id, label: `${citaLabel(c, x)} · saldo ${money(saldoCita(c, x, r?.id))}` })),
      },
      { name: 'monto', label: 'Monto', type: 'number', required: true, min: 0.01, maxValue: 9999999.99, step: 0.01 },
      { name: 'metodoPago', label: 'Método de pago', type: 'select', required: true, options: toOptions(METODOS) },
      { name: 'referencia', label: 'Referencia', max: 100, span: 2, placeholder: 'Número de voucher o transferencia' },
    ],
  },
  {
    key: 'pacientes', title: 'Pacientes', singular: 'paciente', newLabel: 'Nuevo paciente', icon: 'users', group: 'Personas',
    description: 'Datos personales, seguro y expediente de cada paciente.',
    emptyText: 'Registra el primer paciente para agendarle una cita.',
    searchPlaceholder: 'Buscar por nombre, cédula o teléfono…',
    refs: ['seguros'],
    initialSort: (a, b) => fullName(a).localeCompare(fullName(b), 'es'),
    describe: r => fullName(r),
    columns: [
      { label: 'Paciente', value: r => fullName(r), render: r => personCell(r, r.email) },
      { label: 'Cédula', value: r => r.cedula, render: r => h('span', { class: 'nowrap' }, r.cedula) },
      { label: 'Teléfono', value: r => r.telefono, render: r => h('span', { class: 'nowrap' }, r.telefono) },
      { label: 'Edad', num: true, value: r => ageFrom(r.fechaNacimiento), render: r => `${ageFrom(r.fechaNacimiento)} años` },
      { label: 'Sangre', value: r => SANGRE[r.tipoSangre] },
      { label: 'Seguro', value: (r, c) => byId(c.seguros, r.seguroId)?.nombre || 'Sin seguro' },
    ],
    fields: [
      ...nameFields,
      { name: 'cedula', label: 'Cédula', required: true, max: 20, placeholder: '000-0000000-0' },
      { name: 'fechaNacimiento', label: 'Fecha de nacimiento', type: 'date', required: true, maxValue: today() },
      { name: 'sexo', label: 'Sexo', type: 'select', required: true, options: toOptions(SEXOS) },
      { name: 'tipoSangre', label: 'Tipo de sangre', type: 'select', emptyLabel: 'No indicado', options: toOptions(SANGRE) },
      { name: 'telefono', label: 'Teléfono', type: 'tel', required: true, max: 20 },
      emailField,
      { name: 'direccion', label: 'Dirección', max: 250, span: 2 },
      { name: 'seguroId', label: 'Seguro', type: 'select', numeric: true, emptyLabel: 'Sin seguro', options: (c, r) => activeOptions(c.seguros, 'activo', r?.seguroId, s => `${s.nombre} (${s.porcentajeCobertura}%)`) },
      { name: 'numeroAfiliado', label: 'Número de afiliado', max: 50 },
    ],
    actions: row => [{ icon: 'folder', label: 'Ver expediente', onClick: () => openExpediente(row, { SANGRE, ESTADOS: estadoBadge, SEVERIDAD }) }],
  },
  {
    key: 'alergias', title: 'Alergias', singular: 'alergia', newLabel: 'Nueva alergia', icon: 'alert', group: 'Personas',
    description: 'Alergias conocidas de cada paciente.',
    emptyText: 'Registra las alergias conocidas de tus pacientes.',
    searchPlaceholder: 'Buscar por paciente o sustancia…',
    refs: ['pacientes'],
    filters: [{ name: 'severidad', label: 'Toda severidad', value: r => r.severidad, options: toOptions({ Leve: 'Leve', Moderada: 'Moderada', Severa: 'Severa' }) }],
    describe: (r, c) => `la alergia a ${r.sustancia} de ${fullName(byId(c.pacientes, r.pacienteId))}`,
    columns: [
      { label: 'Paciente', value: (r, c) => fullName(byId(c.pacientes, r.pacienteId)), render: (r, c) => personCell(byId(c.pacientes, r.pacienteId)) },
      { label: 'Sustancia', value: r => r.sustancia },
      { label: 'Severidad', value: r => r.severidad, render: r => badge(r.severidad, SEVERIDAD[r.severidad].tone, SEVERIDAD[r.severidad].icon) },
      { label: 'Observación', value: r => r.observacion, render: r => h('span', { class: 'cell-clip', title: r.observacion || '' }, r.observacion || '—') },
    ],
    fields: [
      { name: 'pacienteId', label: 'Paciente', type: 'select', numeric: true, required: true, span: 2, options: c => c.pacientes.map(p => ({ value: p.id, label: `${fullName(p)} · ${p.cedula}` })) },
      { name: 'sustancia', label: 'Sustancia', required: true, max: 150, hint: 'Escribe el principio activo para que la receta pueda detectarla.' },
      { name: 'severidad', label: 'Severidad', type: 'select', required: true, options: toOptions({ Leve: 'Leve', Moderada: 'Moderada', Severa: 'Severa' }) },
      { name: 'observacion', label: 'Observación', type: 'textarea', max: 500, span: 2 },
    ],
  },
  {
    key: 'medicos', title: 'Médicos', singular: 'médico', newLabel: 'Nuevo médico', icon: 'doctor', group: 'Personas',
    description: 'Personal médico, especialidad y costo de consulta.',
    emptyText: 'Registra un médico y luego asígnale un horario.',
    searchPlaceholder: 'Buscar por nombre, licencia o especialidad…',
    refs: ['especialidades'],
    initialSort: (a, b) => fullName(a).localeCompare(fullName(b), 'es'),
    describe: r => fullName(r),
    columns: [
      { label: 'Médico', value: r => fullName(r), render: r => personCell(r, r.email) },
      { label: 'Licencia', value: r => r.licencia },
      { label: 'Especialidad', value: (r, c) => byId(c.especialidades, r.especialidadId)?.nombre },
      { label: 'Teléfono', value: r => r.telefono, render: r => h('span', { class: 'nowrap' }, r.telefono) },
      { label: 'Estado', value: r => (r.activo ? 'Activo' : 'Inactivo'), render: r => activoBadge(r.activo) },
      { label: 'Consulta', num: true, value: r => r.costoConsulta, render: r => money(r.costoConsulta) },
    ],
    fields: [
      ...nameFields,
      { name: 'licencia', label: 'Licencia', required: true, max: 30 },
      { name: 'especialidadId', label: 'Especialidad', type: 'select', numeric: true, required: true, options: (c, r) => activeOptions(c.especialidades, 'activa', r?.especialidadId, e => e.nombre) },
      { name: 'telefono', label: 'Teléfono', type: 'tel', required: true, max: 20 },
      emailField,
      { name: 'costoConsulta', label: 'Costo de la consulta', type: 'number', required: true, min: 0, maxValue: 9999999.99, step: 0.01 },
      { name: 'activo', label: 'Estado', type: 'checkbox', checkLabel: 'Recibe citas' },
    ],
  },
  {
    key: 'horarios', title: 'Horarios', singular: 'horario', newLabel: 'Nuevo horario', icon: 'clock', group: 'Personas',
    description: 'Bloques semanales en los que cada médico recibe citas.',
    emptyText: 'Sin horarios no se pueden agendar citas.',
    searchPlaceholder: 'Buscar por médico o día…',
    refs: ['medicos'],
    initialSort: (a, b) => a.medicoId - b.medicoId
      || ORDEN_DIAS.indexOf(a.diaSemana) - ORDEN_DIAS.indexOf(b.diaSemana)
      || a.horaInicio.localeCompare(b.horaInicio),
    filters: [{ name: 'dia', label: 'Todos los días', value: r => r.diaSemana, options: toOptions(DIAS) }],
    describe: (r, c) => `el horario de ${fullName(byId(c.medicos, r.medicoId))} del ${DIAS[r.diaSemana]}`,
    columns: [
      { label: 'Médico', value: (r, c) => fullName(byId(c.medicos, r.medicoId)), render: (r, c) => personCell(byId(c.medicos, r.medicoId)) },
      { label: 'Día', value: r => DIAS[r.diaSemana] },
      { label: 'Inicio', value: r => r.horaInicio, render: r => formatHour(r.horaInicio) },
      { label: 'Fin', value: r => r.horaFin, render: r => formatHour(r.horaFin) },
      { label: 'Estado', value: r => (r.disponible ? 'Disponible' : 'No disponible'), render: r => (r.disponible ? badge('Disponible', 'good', 'check') : badge('No disponible', 'neutral', 'close')) },
    ],
    fields: [
      { name: 'medicoId', label: 'Médico', type: 'select', numeric: true, required: true, options: (c, r) => activeOptions(c.medicos, 'activo', r?.medicoId, fullName) },
      { name: 'diaSemana', label: 'Día', type: 'select', required: true, options: toOptions(DIAS) },
      { name: 'horaInicio', label: 'Hora de inicio', type: 'time', required: true },
      { name: 'horaFin', label: 'Hora de fin', type: 'time', required: true },
      { name: 'disponible', label: 'Estado', type: 'checkbox', checkLabel: 'Disponible para citas' },
    ],
  },
  {
    key: 'especialidades', title: 'Especialidades', singular: 'especialidad', newLabel: 'Nueva especialidad', icon: 'tag', group: 'Catálogos',
    description: 'Áreas médicas disponibles en la clínica.',
    emptyText: 'Agrega una especialidad para poder registrar médicos.',
    describe: r => r.nombre,
    columns: [
      { label: 'Nombre', value: r => r.nombre },
      { label: 'Descripción', value: r => r.descripcion, render: r => h('span', { class: 'cell-clip', title: r.descripcion || '' }, r.descripcion || '—') },
      { label: 'Estado', value: r => (r.activa ? 'Activa' : 'Inactiva'), render: r => activaBadge(r.activa) },
    ],
    fields: [
      { name: 'nombre', label: 'Nombre', required: true, max: 100 },
      { name: 'activa', label: 'Estado', type: 'checkbox', checkLabel: 'Activa' },
      { name: 'descripcion', label: 'Descripción', type: 'textarea', max: 500, span: 2 },
    ],
  },
  {
    key: 'consultorios', title: 'Consultorios', singular: 'consultorio', newLabel: 'Nuevo consultorio', icon: 'door', group: 'Catálogos',
    description: 'Espacios físicos donde se atienden las citas.',
    emptyText: 'Agrega un consultorio para poder agendar citas.',
    describe: r => r.codigo,
    columns: [
      { label: 'Código', value: r => r.codigo },
      { label: 'Ubicación', value: r => r.ubicacion },
      { label: 'Estado', value: r => (r.activo ? 'Activo' : 'Inactivo'), render: r => activoBadge(r.activo) },
    ],
    fields: [
      { name: 'codigo', label: 'Código', required: true, max: 20, placeholder: 'C-101' },
      { name: 'activo', label: 'Estado', type: 'checkbox', checkLabel: 'Activo' },
      { name: 'ubicacion', label: 'Ubicación', max: 150, span: 2 },
    ],
  },
  {
    key: 'medicamentos', title: 'Medicamentos', singular: 'medicamento', newLabel: 'Nuevo medicamento', icon: 'pill', group: 'Catálogos',
    description: 'Medicamentos que se pueden indicar en una receta.',
    emptyText: 'Agrega medicamentos para poder crear recetas.',
    describe: r => r.nombre,
    columns: [
      { label: 'Nombre', value: r => r.nombre },
      { label: 'Principio activo', value: r => r.principioActivo },
      { label: 'Presentación', value: r => r.presentacion },
      { label: 'Estado', value: r => (r.activo ? 'Activo' : 'Inactivo'), render: r => activoBadge(r.activo) },
    ],
    fields: [
      { name: 'nombre', label: 'Nombre', required: true, max: 150, placeholder: 'Ibuprofeno 400 mg' },
      { name: 'principioActivo', label: 'Principio activo', required: true, max: 150 },
      { name: 'presentacion', label: 'Presentación', max: 100, placeholder: 'Tabletas' },
      { name: 'activo', label: 'Estado', type: 'checkbox', checkLabel: 'Activo' },
    ],
  },
  {
    key: 'seguros', title: 'Seguros', singular: 'seguro', newLabel: 'Nuevo seguro', icon: 'shield', group: 'Catálogos',
    description: 'Aseguradoras y porcentaje que cubren de cada consulta.',
    emptyText: 'Agrega una aseguradora para asignarla a tus pacientes.',
    describe: r => r.nombre,
    columns: [
      { label: 'Nombre', value: r => r.nombre },
      { label: 'Teléfono', value: r => r.telefono },
      { label: 'Estado', value: r => (r.activo ? 'Activo' : 'Inactivo'), render: r => activoBadge(r.activo) },
      { label: 'Cobertura', num: true, value: r => r.porcentajeCobertura, render: r => `${r.porcentajeCobertura}%` },
    ],
    fields: [
      { name: 'nombre', label: 'Nombre', required: true, max: 100 },
      { name: 'telefono', label: 'Teléfono', type: 'tel', max: 20 },
      { name: 'porcentajeCobertura', label: 'Cobertura (%)', type: 'number', required: true, min: 0, maxValue: 100, step: 0.01 },
      { name: 'activo', label: 'Estado', type: 'checkbox', checkLabel: 'Activo' },
    ],
  },
];

export const moduleByKey = key => MODULES.find(m => m.key === key);

async function cambiarEstado(row, estado, reload) {
  try {
    await api.cambiarEstadoCita(row.id, estado);
    toast(`Cita ${estado.toLowerCase()}.`);
    reload();
  } catch (error) {
    toast(error.message, 'error');
  }
}
