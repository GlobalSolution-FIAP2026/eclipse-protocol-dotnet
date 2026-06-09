using GlobalSolution.Data;
using GlobalSolution.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSolution.Service;

public class UsuarioService
{
    private readonly AppDbContext _context;

    public UsuarioService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Usuario>> ListarTodosAsync()
    {
        return await _context.Usuarios
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Usuario?> BuscarPorIdAsync(long id)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.IdUsuario == id);
    }

    public async Task<Usuario> CriarAsync(Usuario usuario)
    {
        var emailExiste = await _context.Usuarios
            .AnyAsync(u => u.Email == usuario.Email);

        if (emailExiste)
            throw new InvalidOperationException("Já existe um usuário cadastrado com este e-mail.");

        usuario.Ativo = true;
        usuario.DataCriacao = DateTime.Now;

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return usuario;
    }

    public async Task<Usuario?> AtualizarAsync(long id, Usuario usuarioAtualizado)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario is null)
            return null;

        var emailExiste = await _context.Usuarios
            .AnyAsync(u => u.Email == usuarioAtualizado.Email && u.IdUsuario != id);

        if (emailExiste)
            throw new InvalidOperationException("Já existe outro usuário cadastrado com este e-mail.");

        usuario.Nome = usuarioAtualizado.Nome;
        usuario.Email = usuarioAtualizado.Email;
        usuario.Senha = usuarioAtualizado.Senha;
        usuario.Ativo = usuarioAtualizado.Ativo;

        await _context.SaveChangesAsync();

        return usuario;
    }

    public async Task<bool> ExcluirAsync(long id)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Propriedades)
            .FirstOrDefaultAsync(u => u.IdUsuario == id);

        if (usuario is null)
            return false;

        if (usuario.Propriedades.Any())
            throw new InvalidOperationException("Não é possível excluir usuário vinculado a propriedades.");

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return true;
    }
}