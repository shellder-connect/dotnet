using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class DadosCadastraisService : IDadosCadastraisService
{
    private readonly IDadosCadastraisRepository _dadosCadastraisRepository;

    public DadosCadastraisService(IDadosCadastraisRepository dadosCadastraisRepository)
    {
        _dadosCadastraisRepository = dadosCadastraisRepository;
    }

    public DadosCadastraisDTO ObterDadosCadastraisPorUsuarioId(string userId)
    {
        var usuario = _dadosCadastraisRepository.ObterUsuarioId(userId);
        var endereco = _dadosCadastraisRepository.ObterEnderecoId(userId);
       
        // Verifica se o usuário existe
        if (usuario == null)
        {
            throw new Exception($"Usuário com ID {userId} não encontrado.");
        }

        return new DadosCadastraisDTO
        {
            Usuario = new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                IdTipoUsuario = usuario.IdTipoUsuario,
                Telefone = usuario.Telefone,
                IdEndereco = usuario.IdEndereco,
                DataNascimento = usuario.DataNascimento,
                Documento = usuario.Documento,
                Status = usuario.Status,
            },
            Endereco = endereco != null && endereco.Rua != null && endereco.CEP != null && endereco.Cidade != null && endereco.Estado != null && endereco.Bairro != null
                ? new EnderecoDTO
                {
                    CEP = endereco.CEP,
                    Estado = endereco.Estado,
                    Cidade = endereco.Cidade,
                    Bairro = endereco.Bairro,
                    Rua = endereco.Rua  
                }
                : new EnderecoDTO(),
        };
    }
}
