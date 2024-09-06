﻿using MediatR;

namespace TechShopSolution.Application.Commands.Products.UpdateProduct;

public class UpdateProductCommand : IRequest<bool>
{
    public int Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string? Slug { get; set; }
    public string? Image { get; set; }
    public string? MoreImages { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal PromotionPrice { get; set; }
    public int Warranty { get; set; }
    public int? Instock { get; set; }
    public string? Specifications { get; set; }
    public string? ShortDesc { get; set; }
    public string? Descriptions { get; set; }
    public bool Featured { get; set; }
    public bool BestSeller { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaKeywords { get; set; }
    public string? MetaDescriptions { get; set; }

}