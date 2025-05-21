// 1) CSS dinámico
const style = document.createElement('style');
style.textContent = `
    .charts-container {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
        gap: 1.5rem;
        margin-bottom: 1.5rem;
    }
    .chart-card {
        position: relative;
        background-color: #ffffff;
        border-radius: 15px;
        padding: 1.5rem;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
        transition: transform 0.3s ease, box-shadow 0.3s ease;
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
        .chart-card { width: 100%; }
        .chart-wrapper { min-height: 250px; }
    }
`;
document.head.appendChild(style);

// Helper para verificar Chart.js
function inicializarChartJs() {
    if (typeof Chart === 'undefined') {
        console.error('Chart.js NO está cargado');
        return false;
    }
    return true;
}

// Limpiar gráficos anteriores (para evitar overlays)
function limpiarGraficos() {
    ['estadosChart', 'citasMesChart', 'clientesChart'].forEach(id => {
        const c = document.getElementById(id);
        if (c && c._chartInstance) {
            c._chartInstance.destroy();
            c._chartInstance = null;
        }
    });
}

window.crearGraficos = function (estLabels, estData, mesLabels, mesData, cliLabels, cliData) {
    console.log('crearGraficos llamado con:', { estLabels, estData, mesLabels, mesData, cliLabels, cliData });

    if (!inicializarChartJs()) return;

    // Verificar existencia de canvas
    ['estadosChart', 'citasMesChart', 'clientesChart'].forEach(id => {
        const c = document.getElementById(id);
        if (!c) console.error('No existe el canvas con ID:', id);
    });

    mostrarMensajesCarga();

    setTimeout(() => {
        limpiarGraficos();
        crearGraficoEstados(estLabels, estData);
        crearGraficoMeses(mesLabels, mesData);
        crearGraficoClientes(cliLabels, cliData);
        ocultarMensajesCarga();
    }, 50);
};

function mostrarMensajesCarga() {
    ['estadosChart', 'citasMesChart', 'clientesChart'].forEach(id => {
        const canvas = document.getElementById(id);
        if (canvas && !document.getElementById(`loading-${id}`)) {
            const loading = document.createElement('div');
            loading.className = 'chart-loading';
            loading.id = `loading-${id}`;
            loading.textContent = 'Cargando gráfico…';
            canvas.parentNode.appendChild(loading);
        }
    });
}

function ocultarMensajesCarga() {
    document.querySelectorAll('.chart-loading').forEach(el => el.remove());
}

function crearGraficoEstados(labels, data) {
    const canvas = document.getElementById('estadosChart');
    if (!canvas) return;
    if (canvas._chartInstance) canvas._chartInstance.destroy();
    const ctx = canvas.getContext('2d');
    canvas._chartInstance = new Chart(ctx, {
        type: 'doughnut',
        data: { labels, datasets: [{ data, backgroundColor: ['#28a745', '#ffc107', '#dc3545'], borderWidth: 1 }] },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { position: 'bottom' },
                tooltip: { callbacks: { label: ctx => `${ctx.label}: ${ctx.raw} citas` } }
            }
        }
    });
}

function crearGraficoMeses(labels, data) {
    const canvas = document.getElementById('citasMesChart');
    if (!canvas) return;
    if (canvas._chartInstance) canvas._chartInstance.destroy();
    const ctx = canvas.getContext('2d');
    canvas._chartInstance = new Chart(ctx, {
        type: 'bar',
        data: {
            labels,
            datasets: [{
                label: 'Citas',
                data,
                backgroundColor: '#6f42c1',
                borderColor: '#4b3c82',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: { y: { beginAtZero: true, ticks: { stepSize: 1 } } },
            plugins: { tooltip: { callbacks: { label: ctx => `${ctx.raw} citas en ${ctx.label}` } } }
        }
    });
}

function crearGraficoClientes(labels, data) {
    const canvas = document.getElementById('clientesChart');
    if (!canvas) return;
    if (canvas._chartInstance) canvas._chartInstance.destroy();
    const ctx = canvas.getContext('2d');
    canvas._chartInstance = new Chart(ctx, {
        type: 'bar',
        data: {
            labels,
            datasets: [{
                label: 'Citas',
                data,
                backgroundColor: '#6f42c1',
                borderColor: '#4b3c82',
                borderWidth: 1
            }]
        },
        options: {
            indexAxis: 'y',
            responsive: true,
            maintainAspectRatio: false,
            scales: { x: { beginAtZero: true, ticks: { stepSize: 1 } } },
            plugins: { legend: { display: false }, tooltip: { callbacks: { label: ctx => `${ctx.raw} citas` } } }
        }
    });
}
