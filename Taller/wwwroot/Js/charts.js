// Crear y agregar estilos dinámicamente
const style = document.createElement('style');
style.textContent = `
    .chart-card {
        position: relative;
        min-height: 300px;
        background-color: #ffffff;
        border-radius: 15px;
        padding: 1.5rem;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
        transition: transform 0.3s ease, box-shadow 0.3s ease;
        margin-bottom: 1.5rem;
    }

    .chart-card:hover {
        transform: translateY(-5px);
        box-shadow: 0 6px 16px rgba(0,0,0,0.12);
    }

    .chart-card h3 {
        margin-top: 0;
        color: #6f42c1;
        font-size: 1.3rem;
        padding-bottom: 0.8rem;
        border-bottom: 1px solid #eee;
        margin-bottom: 1.5rem;
    }

    .chart-wrapper {
        position: relative;
        height: 100%;
        min-height: 300px;
    }

    .chart-card.full-width .chart-wrapper {
        min-height: 400px;
    }

    .chart-loading {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        color: #6c757d;
        font-style: italic;
    }

    @media (max-width: 768px) {
        .chart-card {
            min-width: 100%;
        }
        
        .chart-wrapper {
            height: 250px;
        }
    }
`;
document.head.appendChild(style);

// Función para inicializar Chart.js
function inicializarChartJs() {
    if (typeof Chart === 'undefined') {
        console.error('Chart.js no está cargado correctamente');
        return false;
    }
    return true;
}

// Función principal para crear los gráficos
window.crearGraficos = function (estadosLabels, estadosData, mesesLabels, mesesData, clientesLabels, clientesData) {
    if (!inicializarChartJs()) {
        console.error('No se pueden crear gráficos: Chart.js no está disponible');
        return;
    }

    // Mostrar mensajes de carga mientras se renderizan los gráficos
    mostrarMensajesCarga();

    // Crear gráficos con un pequeño retraso para asegurar la renderización
    setTimeout(() => {
        crearGraficoEstados(estadosLabels, estadosData);
        crearGraficoMeses(mesesLabels, mesesData);
        crearGraficoClientes(clientesLabels, clientesData);

        // Ocultar mensajes de carga
        ocultarMensajesCarga();
    }, 50);
};

function mostrarMensajesCarga() {
    const charts = ['estadosChart', 'citasMesChart', 'clientesChart'];
    charts.forEach(id => {
        const canvas = document.getElementById(id);
        if (canvas) {
            const loadingDiv = document.createElement('div');
            loadingDiv.className = 'chart-loading';
            loadingDiv.textContent = 'Cargando gráfico...';
            loadingDiv.id = `loading-${id}`;
            canvas.parentNode.appendChild(loadingDiv);
        }
    });
}

function ocultarMensajesCarga() {
    const loadings = document.querySelectorAll('.chart-loading');
    loadings.forEach(loading => loading.remove());
}

function crearGraficoEstados(labels, data) {
    const ctx = document.getElementById('estadosChart');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: ['#28a745', '#ffc107', '#dc3545'],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom',
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            return `${context.label}: ${context.raw} citas`;
                        }
                    }
                }
            }
        }
    });
}

function crearGraficoMeses(labels, data) {
    const ctx = document.getElementById('citasMesChart');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Citas',
                data: data,
                backgroundColor: '#6f42c1',
                borderColor: '#4b3c82',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 1
                    }
                }
            },
            plugins: {
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            return `${context.raw} citas en ${context.label}`;
                        }
                    }
                }
            }
        }
    });
}

function crearGraficoClientes(labels, data) {
    const ctx = document.getElementById('clientesChart');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Citas',
                data: data,
                backgroundColor: '#6f42c1',
                borderColor: '#4b3c82',
                borderWidth: 1
            }]
        },
        options: {
            indexAxis: 'y',
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                x: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 1
                    }
                }
            },
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            return `${context.raw} citas`;
                        }
                    }
                }
            }
        }
    });
}