let dadosCompletos = {};

async function carregarDados() {
    try {
        const response = await fetch('http://localhost:3001/api/Proximidade/dashboard');
        if (!response.ok) {
            throw new Error(`Erro HTTP: ${response.status}`);
        }
        
        dadosCompletos = await response.json();
        
        // Atualizar estatísticas
        document.getElementById('totalUsuarios').textContent = dadosCompletos.estatisticas.totalUsuarios;
        document.getElementById('totalAbrigos').textContent = dadosCompletos.estatisticas.totalAbrigos;
        document.getElementById('usuariosComCoordenadas').textContent = dadosCompletos.estatisticas.usuariosComCoordenadas;
        document.getElementById('totalCalculos').textContent = dadosCompletos.estatisticas.totalCalculosProximidade;
        
        // Carregar tabelas
        carregarTabelaUsuarios(dadosCompletos.usuarios);
        carregarTabelaAbrigos(dadosCompletos.abrigos);
        carregarTabelaProximidade(dadosCompletos.proximidade);
        carregarFiltroUsuarios(dadosCompletos.usuarios);
        
    } catch (error) {
        console.error('Erro ao carregar dados:', error);
        mostrarErro('Erro ao carregar dados do servidor: ' + error.message);
    }
}

function carregarTabelaUsuarios(usuarios) {
    const tbody = document.getElementById('corpoTabelaUsuarios');
    const loading = document.getElementById('loadingUsuarios');
    const tabela = document.getElementById('tabelaUsuarios');
    
    tbody.innerHTML = '';
    
    usuarios.forEach(usuario => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td><strong>${usuario.nomeUsuario}</strong></td>
            <td>${usuario.email}</td>
            <td>${usuario.telefone || '-'}</td>
            <td>${usuario.cep}</td>
            <td>${usuario.enderecoCompleto}</td>
            <td class="coordinates">
                ${usuario.latitude && usuario.longitude 
                    ? `${usuario.latitude.toFixed(6)}, ${usuario.longitude.toFixed(6)}` 
                    : 'Não localizado'}
            </td>
        `;
        tbody.appendChild(tr);
    });
    
    loading.style.display = 'none';
    tabela.style.display = 'table';
}

function carregarTabelaAbrigos(abrigos) {
    const tbody = document.getElementById('corpoTabelaAbrigos');
    const loading = document.getElementById('loadingAbrigos');
    const tabela = document.getElementById('tabelaAbrigos');
    
    tbody.innerHTML = '';
    
    abrigos.forEach(abrigo => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td><strong>${abrigo.idAbrigo}</strong></td>
            <td>${abrigo.cep || '-'}</td>
            <td>${abrigo.enderecoCompleto || '-'}</td>
            <td>${abrigo.capacidadeTotal || '-'}</td>
            <td>
                <span class="badge ${abrigo.ocupacaoAtual >= abrigo.capacidadeTotal * 0.8 ? 'badge-warning' : 'badge-success'}">
                    ${abrigo.ocupacaoAtual || 0}/${abrigo.capacidadeTotal || 0}
                </span>
            </td>
            <td class="coordinates">
                ${abrigo.latitude && abrigo.longitude 
                    ? `${abrigo.latitude.toFixed(6)}, ${abrigo.longitude.toFixed(6)}` 
                    : 'Não localizado'}
            </td>
            <td>
                <span class="badge ${abrigo.status === 'Ativo' ? 'badge-success' : 'badge-error'}">
                    ${abrigo.status || 'Desconhecido'}
                </span>
            </td>
        `;
        tbody.appendChild(tr);
    });
    
    loading.style.display = 'none';
    tabela.style.display = 'table';
}

function carregarTabelaProximidade(proximidades) {
    const tbody = document.getElementById('corpoTabelaProximidade');
    const loading = document.getElementById('loadingProximidade');
    const tabela = document.getElementById('tabelaProximidade');
    
    tbody.innerHTML = '';
    
    proximidades.forEach(prox => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td><span class="ranking rank-${prox.ranking <= 3 ? prox.ranking : '4'}">${prox.ranking}º</span></td>
            <td><strong>${prox.nomeUsuario}</strong></td>
            <td>${prox.emailUsuario}</td>
            <td>${prox.enderecoUsuario}</td>
            <td>${prox.enderecoAbrigo}</td>
            <td class="distance">${prox.distanciaKm} km</td>
        `;
        tbody.appendChild(tr);
    });
    
    loading.style.display = 'none';
    tabela.style.display = 'table';
}

function carregarFiltroUsuarios(usuarios) {
    const select = document.getElementById('filtroUsuario');
    select.innerHTML = '<option value="">Todos os usuários</option>';
    
    usuarios.forEach(usuario => {
        if (usuario.nomeUsuario) {
            const option = document.createElement('option');
            option.value = usuario.idUsuario;
            option.textContent = usuario.nomeUsuario;
            select.appendChild(option);
        }
    });
}

function filtrarProximidade() {
    const filtro = document.getElementById('filtroUsuario').value;
    const proximidades = filtro 
        ? dadosCompletos.proximidade.filter(p => p.idUsuario === filtro)
        : dadosCompletos.proximidade;
    
    carregarTabelaProximidade(proximidades);
}

async function calcularProximidades() {
    try {
        const btn = event.target;
        btn.disabled = true;
        btn.textContent = '⏳ Calculando...';
        
        const response = await fetch('http://localhost:3001/api/Proximidade/calcular', {
            method: 'POST'
        });
        
        if (response.ok) {
            alert('✅ Proximidades calculadas com sucesso!');
            await recarregarDados();
        } else {
            const error = await response.json();
            alert(`❌ Erro: ${error.message || 'Falha no cálculo'}`);
        }
    } catch (error) {
        alert(`❌ Erro: ${error.message}`);
    } finally {
        const btn = event.target;
        btn.disabled = false;
        btn.textContent = '🔄 Recalcular Proximidades';
    }
}

function exportarDados() {
    if (!dadosCompletos.proximidade?.length) {
        alert('❌ Não há dados de proximidade para exportar');
        return;
    }
    
    const csv = gerarCSV(dadosCompletos.proximidade);
    baixarCSV(csv, 'proximidade-usuarios-abrigos.csv');
}

function gerarCSV(dados) {
    const headers = ['Usuario', 'Email', 'Endereco Usuario', 'Endereco Abrigo', 'Distancia (km)', 'Ranking'];
    const csvContent = [
        headers.join(','),
        ...dados.map(item => [
            `"${item.nomeUsuario || ''}"`,
            `"${item.emailUsuario || ''}"`,
            `"${item.enderecoUsuario || ''}"`,
            `"${item.enderecoAbrigo || ''}"`,
            item.distanciaKm || 0,
            item.ranking || 0
        ].join(','))
    ].join('\n');
    
    return csvContent;
}

function baixarCSV(csvContent, filename) {
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    link.setAttribute('href', url);
    link.setAttribute('download', filename);
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

async function recarregarDados() {
    await carregarDados();
}

function mostrarErro(mensagem) {
    const errorDiv = document.createElement('div');
    //errorDiv.className = 'error';
    //errorDiv.textContent = mensagem;
    document.querySelector('.container').prepend(errorDiv);
}

// Carregar dados quando a página carrega
document.addEventListener('DOMContentLoaded', carregarDados);