using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhomPhanQuyenController : ControllerBase
    {

        // gen context
        private readonly QuanTriHeThongContext _context;

        // constructor for context
        public NhomPhanQuyenController(QuanTriHeThongContext context)
        {
            _context = context;
        }

        // exist
        private bool NhomPhanQuyenExists(string id)
        {
            return _context.NhomPhanQuyen.Any(e => e.MaNhomPhanQuyen == id);
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhomPhanQuyen>>> GetNhomPhanQuyen()
        {
            return await _context.NhomPhanQuyen.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NhomPhanQuyen>> GetNhomPhanQuyen(string id)
        {
            var NhomPhanQuyen = await _context.NhomPhanQuyen.FindAsync(id);

            if (NhomPhanQuyen == null)
            {
                return NotFound();
            }

            return NhomPhanQuyen;
        }

        // post 
        [HttpPost]
        public async Task<ActionResult<NhomPhanQuyenDTO>> PostNhomPhanQuyen(NhomPhanQuyenDTO NhomPhanQuyen)
        {

            string maNhomPhanQuyen = NhomPhanQuyen.MaNhomPhanQuyen;
            string tenNhomPhanQuyen = NhomPhanQuyen.TenNhomPhanQuyen;
            string moTa = NhomPhanQuyen.MoTa;
            string trangThai = NhomPhanQuyen.TrangThai;

            if (maNhomPhanQuyen == null || maNhomPhanQuyen.Length == 0 || maNhomPhanQuyen.Length > 20)
            {
                return BadRequest("Mã nhóm phân quyền không hợp lệ!");
            }   

            if (tenNhomPhanQuyen == null || tenNhomPhanQuyen.Length == 0 || tenNhomPhanQuyen.Length > 50)
            {
                return BadRequest("Tên nhóm phân quyền không hợp lệ!");
            }
            
            if (!Utils.Utils.IsValidTrangThai(trangThai))
            {
                return BadRequest("Trạng thái không hợp lệ!");
            }
                

            string query = "INSERT INTO NhomPhanQuyen (MaNhomPhanQuyen, TenNhomPhanQuyen, MoTa, TrangThai) VALUES (@p0, @p1, @p2, @p3)";
            _context.Database.ExecuteSqlRaw(query, maNhomPhanQuyen,tenNhomPhanQuyen, moTa, trangThai);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NhomPhanQuyenExists(NhomPhanQuyen.MaNhomPhanQuyen))
                {
                    return BadRequest("Mã nhóm phân quyền đã tồn tại!");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetNhomPhanQuyen", new { id = NhomPhanQuyen.MaNhomPhanQuyen }, NhomPhanQuyen);
        }

        // put
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhomPhanQuyen(string id, NhomPhanQuyen NhomPhanQuyen)
        {

            string maNhomPhanQuyen = NhomPhanQuyen.MaNhomPhanQuyen;
            string tenNhomPhanQuyen = NhomPhanQuyen.TenNhomPhanQuyen;
            string moTa = NhomPhanQuyen.MoTa;
            string trangThai = NhomPhanQuyen.TrangThai;

            if (id != NhomPhanQuyen.MaNhomPhanQuyen)
            {
                return BadRequest("Mã nhóm phân quyền không hợp lệ!");
            }

            if (maNhomPhanQuyen == null || maNhomPhanQuyen.Length == 0 || maNhomPhanQuyen.Length > 20)
            {
                return BadRequest("Mã nhóm phân quyền không hợp lệ!");
            }

            if (tenNhomPhanQuyen == null || tenNhomPhanQuyen.Length == 0 || tenNhomPhanQuyen.Length > 50)
            {
                return BadRequest("Tên nhóm phân quyền không hợp lệ!");
            }

            if (!Utils.Utils.IsValidTrangThai(trangThai))
            {
                return BadRequest("Trạng thái không hợp lệ!");
            }

            string query = "UPDATE NhomPhanQuyen set TenNhomPhanQuyen = @p1,MoTa = @p2, TrangThai = @p3" +
                " where MaNhomPhanQuyen = @p0";

            _context.Database.ExecuteSqlRaw(query, maNhomPhanQuyen, tenNhomPhanQuyen, moTa, trangThai);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhomPhanQuyenExists(id))
                {
                    return BadRequest("Mã nhóm phân quyền không tồn tại!");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // delete
        [HttpDelete("{id}")]    
        public async Task<IActionResult> DeleteNhomPhanQuyen(string id)
        {
            var NhomPhanQuyen = await _context.NhomPhanQuyen.FindAsync(id);
            if (NhomPhanQuyen == null)
            {
                return NotFound();
            }

            _context.NhomPhanQuyen.Remove(NhomPhanQuyen);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
