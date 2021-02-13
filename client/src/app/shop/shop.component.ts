import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IBrand } from '../shared/models/brands';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productType';
import { ShopService } from './shop.service';
import {ShopParams} from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {

  products: IProduct[];
  brands: IBrand[];
  types: IType[];
  shopParams = new ShopParams();
  totalCount = 0;
  @ViewChild('search', {static: false}) searchTerm : ElementRef; // to use #search template ref var inside here.
 
  sortOptions = [
    {name: 'Alphabetical', value:'name'},
    {name: 'Price: Low to High', value:'priceAsc'},
    {name: 'Price: High to Low', value:'priceDesc'}
  ];

  constructor(private shopService: ShopService ) { }

  ngOnInit() {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts(){
    this.shopService.getProducts(this.shopParams).subscribe((response: IPagination) => {
      this.products = response.data;
      this.shopParams.pageNumber = response.pageIndex;
      this.shopParams.pageSize = response.pageSize;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    });
  }

  getBrands(){
    this.shopService.getBrands().subscribe((response: IBrand[]) => {
      this.brands = [{id:0, name: 'All'}, ...response];
    }, error => {
      console.log(error);
    });
  }
  

  getTypes(){
    this.shopService.getTypes().subscribe((response: IType[]) => {
      this.types = [{id:0, name: 'All'}, ...response];;
    }, error => {
      console.log(error);
    });
  }

  
  onBrandSelected(brandId: number) {
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onTypeSelected(typeId: number) {
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onSortSelected(sort: string) {
    this.shopParams.sort = sort;
    this.getProducts();
  }
  
  onPageChanged(event: any) {
    if(this.shopParams.pageNumber !== event.page){
      this.shopParams.pageNumber = event;
      this.getProducts();
    }
  }

  onSearch() {
    debugger;
    this.shopParams.search = this.searchTerm.nativeElement.value;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onReset(){
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProducts();
  }


  

}
