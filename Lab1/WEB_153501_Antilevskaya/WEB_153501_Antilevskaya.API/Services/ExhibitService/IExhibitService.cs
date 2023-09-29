﻿using WEB_153501_Antilevskaya.Domain.Models;
using WEB_153501_Antilevskaya.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WEB_153501_Antilevskaya.API.Services.ExhibitService;
public interface IExhibitService
{ 
    public Task<ResponseData<ListModel<Exhibit>>> GetExhibitListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);

    /* Create a little bit later
    public Task<ResponseData<Exhibit>> GetExhibitByIdAsync(int id);
    public Task UpdateExhibitAsync(int id, Exhibit exhibit, IFormFile? formFile);
    public Task TaskDeleteExhibitAsync(int id);
    public Task<ResponseData<Exhibit>> CreateExhibitAsync(Exhibit exhibit, IFormFile? formFile);
    public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
*/
}
