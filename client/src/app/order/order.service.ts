import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getOrdersForUser(): any {
    return this.http.get(this.baseUrl + 'orders');
  }

  getOrderDetailed(id: number): any {
    return this.http.get(this.baseUrl + 'orders/' + id);
  }

}
