using MicroServices.Interview.Personnel.API.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MicroServices.Interview.Personnel.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class PersonnelController : ControllerBase
    {
        #region Private variables

        private readonly IPersonnelRepository _personnelRepository;

        #endregion Private variables

        #region Constructor

        public PersonnelController(IPersonnelRepository personnelRepository)
        {
            _personnelRepository = personnelRepository ?? throw new ArgumentNullException(nameof(personnelRepository));
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Tüm personel listesini döndürür
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Model.Personnel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult<IEnumerable<Model.Personnel>> Get()
        {
            /*
             burada direk tüm modeli dönmek yerine sayfalama yapılabilirdi ortak bir model oluşturup
             toplam kayıt sayısı sayfa numarası items gibi alanlarla dönmek daha doğru olurdu
            */
            IEnumerable<Model.Personnel> personnels = _personnelRepository.GetAllPersonnels();

            if (personnels == null || personnels.ToList().Count == 0)
            {
                return NotFound();
            }

            return Ok(personnels);
        }

        /// <summary>
        /// Id değerine göre bir adet personel getirir
        /// </summary>
        /// <param name="id">Personel id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Tables.Personnel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult<Tables.Personnel> Get(int id)
        {
            if (id <= 0)
                return BadRequest();

            Tables.Personnel personnel = _personnelRepository.GetById(id);

            if (personnel == null)
            {
                return NotFound();
            }

            return Ok(new Model.Personnel
            {
                Age = personnel.Age,
                City = personnel.City,
                FullName = personnel.FullName,
                PersonnelId = personnel.PersonnelId
            });
        }

        /// <summary>
        /// Personel ekle
        /// </summary>
        /// <param name="personnel">Personel bilgileri</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Post([FromBody] Model.Personnel personnel)
        {
            if (personnel == null)
                return BadRequest();

            bool isSuccess = _personnelRepository.Insert(ModelToTable(personnel));

            if (!isSuccess)
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Personel güncelle
        /// </summary>
        /// <param name="personnel">Personel bilgileri</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Put([FromBody]Model.Personnel personnel)
        {
            if (personnel == null)
                return BadRequest();

            bool isSuccess = _personnelRepository.Update(ModelToTable(personnel));

            if (!isSuccess)
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Personel sil
        /// </summary>
        /// <param name="id">personel id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            bool isSuccess = _personnelRepository.Delete(id);

            if (!isSuccess)
            {
                return BadRequest();
            }

            return Ok();
        }

        private Tables.Personnel ModelToTable(Model.Personnel personnel)
        {
            // TODO: burada model ile table sınıflarınnı auto mapper için bir kütüphane kullanılabilirdi
            return new Tables.Personnel
            {
                Age = personnel.Age,
                City = personnel.City,
                FullName = personnel.FullName,
                PersonnelId = personnel.PersonnelId
            };
        }

        #endregion Methods
    }
}