using APIBookstore.Models;

namespace APIBookstore.DTOs.Mappings;

public static class CategoriaMappingExtensions
{
    public static CategoriaDTO ToCategoriaDTO(Categoria categoria)
    {
        if (categoria is null)
        {
            return null;
        }
        
        return new CategoriaDTO()
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl
        };
    }
    
    public static Categoria ToCategoria(CategoriaDTO categoriaDto)
    {
        return new Categoria()
        {
            CategoriaId = categoriaDto.CategoriaId,
            Nome = categoriaDto.Nome,
            ImagemUrl = categoriaDto.ImagemUrl
        };
    }

    public static IEnumerable<CategoriaDTO> ToCategoriaDtoList(IEnumerable<Categoria> categorias)
    {
        if (categorias is null || !categorias.Any())
        {
            return new List<CategoriaDTO>();
        }

        return categorias.Select(item => new CategoriaDTO()
        {
            CategoriaId = item.CategoriaId,
            Nome = item.Nome,
            ImagemUrl = item.ImagemUrl
        }).ToList();
    }
    
}