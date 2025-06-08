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
        color: #1E3A8A; /* Azul oscuro para títulos */
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
        data: {
            labels,
            datasets: [{
                data,
                backgroundColor: [
                    '#1E3A8A', // Azul oscuro - Total
                    '#10B981', // Verde - Confirmadas
                    '#F59E0B', // Amarillo - Pendientes
                    '#EF4444'  // Rojo - Canceladas
                ],
                borderColor: [
                    '#1E3A8A', // Azul oscuro
                    '#0D9C6F', // Verde oscuro
                    '#D48A08', // Amarillo oscuro
                    '#D33833'  // Rojo oscuro
                ],
                borderWidth: 1
            }]
        },
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
                backgroundColor: '#1E3A8A', // Azul oscuro
                borderColor: '#142B6B',       // Azul más oscuro
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
                backgroundColor: '#1E3A8A', // Azul oscuro
                borderColor: '#142B6B',     // Azul más oscuro
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

    //graficos excel
    window.exportarGraficosExcel = function () {
        let wb = XLSX.utils.book_new();

        function crearHojaConEstilo(titulo, encabezados, filas, color) {
            let hoja = [encabezados, ...filas];
            let ws = XLSX.utils.aoa_to_sheet(hoja);

            // Agrega un título encima
            XLSX.utils.sheet_add_aoa(ws, [[titulo]], { origin: "A1" });

            // Formato de encabezados (segunda fila)
            if (!ws["!cols"]) ws["!cols"] = [];
            encabezados.forEach((_, i) => ws["!cols"].push({ wch: 22 }));

            // Formato personalizado usando cell styles
            encabezados.forEach((_, i) => {
                let cellRef = XLSX.utils.encode_cell({ c: i, r: 1 });
                if (ws[cellRef]) ws[cellRef].s = { fill: { fgColor: { rgb: color } }, font: { bold: true } };
            });

            return ws;
        }

        // 1. Estados
        let estadosChart = Chart.getChart('estadosChart');
        if (estadosChart) {
            let filas = estadosChart.data.labels.map((label, idx) => [label, estadosChart.data.datasets[0].data[idx]]);
            let ws = crearHojaConEstilo("Resumen de Estados de Citas", ["Estado", "Total"], filas, "1E3A8A");
            XLSX.utils.book_append_sheet(wb, ws, "Estados");
        }

        // 2. Meses
        let mesesChart = Chart.getChart('citasMesChart');
        if (mesesChart) {
            let filas = mesesChart.data.labels.map((label, idx) => [label, mesesChart.data.datasets[0].data[idx]]);
            let ws = crearHojaConEstilo("Resumen de Citas por Mes", ["Mes", "Total"], filas, "1E3A8A");
            XLSX.utils.book_append_sheet(wb, ws, "CitasMes");
        }

        // 3. Clientes
        let clientesChart = Chart.getChart('clientesChart');
        if (clientesChart) {
            let filas = clientesChart.data.labels.map((label, idx) => [label, clientesChart.data.datasets[0].data[idx]]);
            let ws = crearHojaConEstilo("Top 10 Clientes", ["Cliente", "Total Citas"], filas, "1E3A8A");
            XLSX.utils.book_append_sheet(wb, ws, "TopClientes");
        }

        XLSX.writeFile(wb, "GraficosCitas.xlsx");
    };

    //graficos PDF
    window.exportarGraficosPDF = function () {
        const { jsPDF } = window.jspdf;
        let doc = new jsPDF("landscape");

        // --- PORTADA ---
        let y = 25;
        let logoImg = new Image();
        logoImg.src = "/images/logotuning.png";

        logoImg.onload = function () {
            // LOGO grande y centrado
            doc.addImage(logoImg, "PNG", 120, y, 50, 50);

            // TÍTULO y DESCRIPCIÓN centrados
            doc.setFontSize(22);
            doc.setTextColor("#1E3A8A"); // Azul oscuro
            doc.text("Reporte de Citas Agendadas", 148, y + 65, { align: "center" });

            doc.setFontSize(14);
            doc.setTextColor("#444");
            doc.text("Taller Motos Tuning", 148, y + 75, { align: "center" });
            doc.setFontSize(11);
            doc.setTextColor("#333");
            doc.text("Especialistas en servicios, diagnósticos y modificaciones de motos.", 148, y + 83, { align: "center" });
            doc.text("Dirección: Del puente del instituto inep 75 varas al sur, Matagalpa, Nicaragua. Tel: 8832-9893", 148, y + 91, { align: "center" });

            // Fecha de reporte
            doc.setFontSize(10);
            doc.setTextColor("#888");
            doc.text("Fecha: " + (new Date()).toLocaleDateString(), 258, 200);

            // ---- GRÁFICOS EN PÁGINAS SEPARADAS ----

            // 1. ESTADOS
            doc.addPage();
            y = 20;
            addChartToPDF("estadosChart", "Estados de las Citas", "#1E3A8A");

            // 2. CITAS POR MES
            doc.addPage();
            y = 20;
            addChartToPDF("citasMesChart", "Citas por Mes", "#1E3A8A");

            // 3. TOP CLIENTES
            doc.addPage();
            y = 20;
            addChartToPDF("clientesChart", "Top 10 Clientes con más Citas", "#1E3A8A");

            // PIE DE PÁGINA en todas las páginas
            let pageCount = doc.internal.getNumberOfPages();
            for (let i = 1; i <= pageCount; i++) {
                doc.setPage(i);
                doc.setFontSize(10);
                doc.setTextColor("#888");
                doc.text("Reporte generado por el Sistema de Taller Motos Tuning - " + (new Date()).toLocaleDateString(), 15, 200);
                doc.text(`Página ${i} de ${pageCount}`, 270, 200, { align: "right" });
            }

            doc.save("GraficosCitas.pdf");
        };

        // Si ya está en caché
        if (logoImg.complete) logoImg.onload();

        // Función auxiliar para insertar gráfico con su título
        function addChartToPDF(chartId, title, color) {
            let chart = document.getElementById(chartId);
            let chartInstance = Chart.getChart(chartId);
            if (chart && chartInstance) {
                doc.setFontSize(16);
                doc.setTextColor(color);
                doc.text(title, 148, y + 10, { align: "center" });

                // Gráfico como imagen, centrado y con borde
                let img = chart.toDataURL("image/png", 1.0);
                let imgWidth = 180, imgHeight = 80, x = (297 - imgWidth) / 2;
                doc.setDrawColor(180, 180, 180);
                doc.rect(x - 3, y + 16, imgWidth + 6, imgHeight + 6, "S");
                doc.addImage(img, "PNG", x, y + 19, imgWidth, imgHeight);
            }
        }
    };
}