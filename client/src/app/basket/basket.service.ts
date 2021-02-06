import { Injectable } from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {Basket, IBasket, IBasketItem} from '../shared/models/basket';
import {IProduct} from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  baseUrl = environment.apiUrl;

  // BehaviorSubject needs initial value to emit
  private basketSource = new BehaviorSubject<IBasket>(null);

  // added $ as to understand this as observable.
  basket$ = this.basketSource.asObservable();

  constructor(private http: HttpClient) { }

  getBasket(id: string): Observable<any> {
    // get the basket and set inside basketSource
    // doing here to persist basket
    return this.http.get<IBasket>(this.baseUrl + 'basket?id=' + id)
    .pipe(
            map((basket: IBasket) => {
              this.basketSource.next(basket);
              console.log(this.getCurrentBasketValue());
            })
    );
  }

  setBasket(basket: IBasket): any {
    return this.http.post<IBasket>(this.baseUrl + 'basket', basket).subscribe((response: IBasket) => {
      this.basketSource.next(response);
    }, error => {
      console.log(error);
    });
  }

  deleteBasket(id: string): any {
    return this.http.delete(this.baseUrl + 'basket?id=' + id);
  }

  addItemToBasket(item: IProduct, quantity = 1): any {
    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(item, quantity);
    const basket = this.getCurrentBasketValue() ?? this.createBasket(); // using collasing operator
                                    // of new typescript to check if basket is null then create new.
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }

  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    debugger;
    const index = items.findIndex(i => i.id === itemToAdd.id); // find index id -1 means item not found
    if (index === -1) {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    } else {
      // it means item found in basket.
      items[index].quantity += quantity; // increase quantity in case of product match
    }

    return items;
  }

  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  getCurrentBasketValue(): IBasket {
    return this.basketSource.value;
  }


  // for quantity we dont need to map as both name same.
 private mapProductItemToBasketItem(item: IProduct, quantity: number): any {
  return {
    id: item.id,
    productName: item.name,
    price: item.price,
    pictureUrl: item.pictureUrl,
    quantity,
    brand: item.productBrand,
    type: item.productType
  };
 }



}
