using GlobalSolution.Data;
using GlobalSolution.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSolution.Service;

public class PropriedadeService
{
    private readonly AppDbContext _context;

    public PropriedadeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Propriedade>> ListarTodosAsync()
    {
        return await _context.Propriedades
            .Include(p => p.Usuario)
            .Include(p => p.Localizacao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Propriedade?> BuscarPorIdAsync(long id)
    {
        return await _context.Propriedades
            .Include(p => p.Usuario)
            .Include(p => p.Localizacao)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdPropriedade == id);
    }

    public async Task<Propriedade> CriarAsync(Propriedade propriedade)
    {
        var usuarioExiste = await _context.Usuarios
            .AnyAsync(u => u.IdUsuario == propriedade.IdUsuario);

        if (!usuarioExiste)
            throw new InvalidOperationException("Usuário informado não existe.");

        var localizacaoExiste = await _context.Localizacoes
            .AnyAsync(l => l.IdLocalizacao == propriedade.IdLocalizacao);

        if (!localizacaoExiste)
            throw new InvalidOperationException("Localização informada não existe.");

        _context.Propriedades.Add(propriedade);
        await _context.SaveChangesAsync();

        return propriedade;
    }

    public async Task<Propriedade?> AtualizarAsync(long id, Propriedade propriedadeAtualizada)
    {
        var propriedade = await _context.Propriedades.FindAsync(id);

        if (propriedade is null)
            return null;

        var usuarioExiste = await _context.Usuarios
            .AnyAsync(u => u.IdUsuario == propriedadeAtualizada.IdUsuario);

        if (!usuarioExiste)
            throw new InvalidOperationException("Usuário informado não existe.");

        var localizacaoExiste = await _context.Localizacoes
            .AnyAsync(l => l.IdLocalizacao == propriedadeAtualizada.IdLocalizacao);

        if (!localizacaoExiste)
            throw new InvalidOperationException("Localização informada não existe.");

        propriedade.Nome = propriedadeAtualizada.Nome;
        propriedade.AreaTotal = propriedadeAtualizada.AreaTotal;
        propriedade.TipoSolo = propriedadeAtualizada.TipoSolo;
        propriedade.IdUsuario = propriedadeAtualizada.IdUsuario;
        propriedade.IdLocalizacao = propriedadeAtualizada.IdLocalizacao;

        await _context.SaveChangesAsync();

        return propriedade;
    }

    public async Task<bool> ExcluirAsync(long id)
    {
        var propriedade = await _context.Propriedades
            .Include(p => p.Plantacoes)
            .FirstOrDefaultAsync(p => p.IdPropriedade == id);

        if (propriedade is null)
            return false;

        if (propriedade.Plantacoes.Any())
            throw new InvalidOperationException("Não é possível excluir propriedade vinculada a plantações.");

        _context.Propriedades.Remove(propriedade);
        await _context.SaveChangesAsync();

        return true;
    }
}