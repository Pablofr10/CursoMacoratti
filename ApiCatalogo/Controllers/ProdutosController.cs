using ApiCatalogo.DTOs;
using ApiCatalogo.Models;
using ApiCatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ApiCatalogo.Controllers
{
    [Route("api/[Controller]/")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {

        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork contexo, IMapper mapper)
        {
            _uof = contexo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDto>> Get()
        {
            var produtos = _uof.ProdutoRepository.Get().ToList();
            var produtosDto = _mapper.Map<List<ProdutoDto>>(produtos);

            return produtosDto;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPorPreco()
        {
            return _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<ProdutoDto> Get(int id)
        {

            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if (produto == null) return NotFound("Produto não encontrado");

            var produtoDto = _mapper.Map<ProdutoDto>(produto);

            return produtoDto;

        }

        [HttpPost]
        public ActionResult Post([FromBody]ProdutoDto model)
        {
            var produto = _mapper.Map<Produto>(model);
            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            var produtoDto = _mapper.Map<ProdutoDto>(produto);

            return Created($"api/produtos/{produto.ProdutoId}", produtoDto);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody]ProdutoDto model)
        {
            if (id != model.ProdutoId)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(model);

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            var produtoDto = _mapper.Map<ProdutoDto>(produto);

            return Ok(produtoDto);
        }

        [HttpDelete("{id}")]
        public ActionResult<ProdutoDto> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if (produto == null) return NotFound("Produto não encontrado");

            var produtoDto = _mapper.Map<ProdutoDto>(produto);

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            return produtoDto;
        }
    }
}
