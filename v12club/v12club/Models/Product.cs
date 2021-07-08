using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace v12club.Models
{
	//public class Offer
	//{
	//	[JsonProperty("@type")]
	//	public string Type { get; set; }
	//	public string url { get; set; }
	//	public string price { get; set; }
	//	public string priceCurrency { get; set; }
	//	public string availability { get; set; }
	//	public string highPrice { get; set; }
	//	public string lowPrice { get; set; }
	//	public int offerCount { get; set; }
	//	public List<Offer> offers { get; set; }
	//}

	//public class Item
	//{
	//	[JsonProperty("@type")]
	//	public string Type { get; set; }
	//	public string url { get; set; }
	//	public string manufacturer { get; set; }
	//	public string brand { get; set; }
	//	public string mpn { get; set; }
	//	public string name { get; set; }
	//	public string description { get; set; }
	//	public string image { get; set; }
	//	public Offers offers { get; set; }
	//	public string gtin13 { get; set; }
	//}

	//public class ItemListElement
	//{
	//	[JsonProperty("@type")]
	//	public string Type { get; set; }
	//	public int position { get; set; }
	//	public Item item { get; set; }
	//}

	//public class Root
	//{
	//	[JsonProperty("@context")]
	//	public string Context { get; set; }

	//	[JsonProperty("@type")]
	//	public string Type { get; set; }
	//	public string url { get; set; }
	//	public int numberOfItems { get; set; }
	//	public List<ItemListElement> itemListElement { get; set; }
	//}
}
public class Products
{
	public string context { get; set; }
	public string type { get; set; }
	public string url { get; set; }
	public int numberOfItems { get; set; }
	public ProductElement[] itemListElement { get; set; }
}

public class ProductElement
{
	public string type { get; set; }
	public int position { get; set; }
	public Product item { get; set; }
}

public class Product
{
	public string type { get; set; }
	public string url { get; set; }
	public string manufacturer { get; set; }
	public string brand { get; set; }
	public string mpn { get; set; }
	public string name { get; set; }
	public string description { get; set; }
	public string image { get; set; }
	public Offers offers { get; set; }
}

public class Offers
{
	public string type { get; set; }
	public string highPrice { get; set; }
	public string lowPrice { get; set; }
	public string priceCurrency { get; set; }
	public int offerCount { get; set; }
	public Offer[] offers { get; set; }
	public string url { get; set; }
	public string price { get; set; }
	public string availability { get; set; }
}

public class Offer
{
	public string type { get; set; }
	public string url { get; set; }
	public string price { get; set; }
	public string priceCurrency { get; set; }
	public string availability { get; set; }
}

