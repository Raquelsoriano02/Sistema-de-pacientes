import { API_URL } from './config.js';

// Clave usada en el frontend -> nombre del controlador en la API.
export const RESOURCES = {
  seguros: 'Seguro',
  especialidades: 'Especialidad',
  consultorios: 'Consultorio',
  medicamentos: 'Medicamento',
  medicos: 'Medico',
  horarios: 'Horario',
  pacientes: 'Paciente',
  alergias: 'Alergia',
  citas: 'Cita',
  consultas: 'Consulta',
  recetas: 'Receta',
  pagos: 'Pago',
};

async function leerError(response) {
  if (response.status >= 500) {
    return `La API tuvo un error interno (${response.status}). Revisa la consola de la API.`;
  }

  const texto = await response.text();
  try {
    const problema = JSON.parse(texto);
    if (problema.errors) return Object.values(problema.errors).flat().join(' ');
    return problema.title || texto;
  } catch {
    return texto || `La solicitud falló (${response.status}).`;
  }
}

async function request(path, options = {}) {
  let response;
  try {
    response = await fetch(`${API_URL}${path}`, {
      ...options,
      headers: options.body ? { 'Content-Type': 'application/json' } : undefined,
    });
  } catch {
    throw new Error(`No se pudo conectar con la API en ${API_URL}. Verifica que esté en ejecución.`);
  }

  if (!response.ok) throw new Error(await leerError(response));
  return response.status === 204 ? null : response.json();
}

export const api = {
  list: key => request(`/${RESOURCES[key]}`),
  create: (key, data) => request(`/${RESOURCES[key]}`, { method: 'POST', body: JSON.stringify(data) }),
  update: (key, id, data) => request(`/${RESOURCES[key]}/${id}`, { method: 'PUT', body: JSON.stringify(data) }),
  remove: (key, id) => request(`/${RESOURCES[key]}/${id}`, { method: 'DELETE' }),
  disponibilidad: (medicoId, consultorioId, fecha) =>
    request(`/Cita/disponibilidad?medicoId=${medicoId}&consultorioId=${consultorioId}&fecha=${fecha}`),
  cambiarEstadoCita: (id, estado) =>
    request(`/Cita/${id}/estado`, { method: 'PUT', body: JSON.stringify(estado) }),
};

// Carga varias listas a la vez: { pacientes: [...], medicos: [...] }.
export async function loadAll(keys) {
  const unicos = [...new Set(keys)];
  const listas = await Promise.all(unicos.map(key => api.list(key)));
  return Object.fromEntries(unicos.map((key, i) => [key, listas[i]]));
}
