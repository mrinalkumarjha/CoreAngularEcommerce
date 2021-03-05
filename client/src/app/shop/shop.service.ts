import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import {IPagination} from '../shared/models/pagination';
import {IBrand} from '../shared/models/brands';
import {IType} from '../shared/models/productType';
import {map} from 'rxjs/operators';
import {ShopParams} from '../shared/models/shopParams';
import { IProduct } from '../shared/models/product';
import { of } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class ShopService {

  private baseUrl: string = 'https://localhost:5001/api/';
  products: IProduct[] = [];
  brands: IBrand[] = [];
  types: IType[] = [];
  constructor(private http: HttpClient) { }


  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();

    if (shopParams.brandId !== 0){
      params = params.append('brandId', shopParams.brandId.toString());
    }
    if (shopParams.typeId !== 0){
      params = params.append('typeId', shopParams.typeId.toString());
    }
    if (shopParams.search){
      params = params.append('search', shopParams.search);
    }
    params = params.append('sort', shopParams.sort.toString());
    params = params.append('pageIndex', shopParams.pageNumber.toString());
    params = params.append('pageSize', shopParams.pageSize.toString());

    return this.http.get<IPagination>(this.baseUrl + 'products', {observe: 'response', params})
    .pipe(
           map(response => {
             this.products = response.body.data; // for clients caching purpose
             return response.body;
           })
    )
  }

  // Here we are checking if product available in inmemory client then get it from there else go to server.
  getProduct(id: number) {
    const product = this.products.find(p => p.id === id);
    if (product){
      return of(product);
    }
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }

  getBrands() {
    if (this.brands.length > 0)
    {
      return of(this.brands);
    }
    return this.http.get<IBrand[]>(this.baseUrl + 'products/brands').pipe(
      map(response => {
        this.brands = response; // for client caching purpose we are storing in clinet
        return response;
      })
    )
  }

  getTypes() {
    if (this.types.length > 0)
    {
      return of(this.types);
    }
    return this.http.get<IType[]>(this.baseUrl + 'products/types').pipe(
      map(response => {
        this.types = response;
        return response;
      })
    )
  }

}
