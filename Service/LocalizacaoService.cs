using GlobalSolution.Data;
using GlobalSolution.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSolution.Services;

public class LocalizacaoService
{
    private readonly AppDbContext _context;

    public LocalizacaoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Localizacao>> ListarTodosAsync()
    {
        return await _context.Localizacoes
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Localizacao?> BuscarPorIdAsync(long id)
    {
        return await _context.Localizacoes
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.IdLocalizacao == id);
    }

    public async Task<Localizacao> CriarAsync(Localizacao localizacao)
    {
        _context.Localizacoes.Add(localizacao);
        await _context.SaveChangesAsync();

        return localizacao;
    }

    public async Task<Localizacao?> AtualizarAsync(long id, Localizacao localizacaoAtualizada)
    {
        var localizacao = await _context.Localizacoes.FindAsync(id);

        if (localizacao is null)
            return null;

        localizacao.Cidade = localizacaoAtualizada.Cidade;
        localizacao.Estado = localizacaoAtualizada.Estado;
        localizacao.Pais = localizacaoAtualizada.Pais;
        localizacao.Latitude = localizacaoAtualizada.Latitude;
        localizacao.Longitude = localizacaoAtualizada.Longitude;
        localizacao.Cep = localizacaoAtualizada.Cep;

        await _context.SaveChangesAsync();

        return localizacao;
    }

    public async Task<bool> ExcluirAsync(long id)
    {
        var localizacao = await _context.Localizacoes
            .Include(l => l.Propriedades)
            .FirstOrDefaultAsync(l => l.IdLocalizacao == id);

        if (localizacao is null)
            return false;

        if (localizacao.Propriedades.Any())
            throw new InvalidOperationException("Não é possível excluir localização vinculada a propriedades.");

        _context.Localizacoes.Remove(localizacao);
        await _context.SaveChangesAsync();

        return true;
    }
}