import { Injectable } from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {map} from 'rxjs/operators';
import { IOrderToCreate } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  createOrder(order: IOrderToCreate){
    return this.http.post(this.baseUrl + 'orders', order);
  }

  getDeliveryMethods(){
    return this.http.get(this.baseUrl + 'orders/deliveryMethods')
    .pipe(
        map((delMethods: any) => {
          return delMethods.sort((a, b) => b.price - a.price)
        })
    );
  }

}
