import { AfterViewInit, Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/shared/models/basket';
import { IOrder } from 'src/app/shared/models/order';
import { CheckoutService } from '../checkout.service';

declare var Stripe;

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements AfterViewInit, OnDestroy {

  @Input() checkoutForm: FormGroup;
  @ViewChild('cardNumber', {static: true}) cardNumberElement: ElementRef;
  @ViewChild('cardExpiry', {static: true}) cardExpiryElement: ElementRef;
  @ViewChild('cardCvc', {static: true}) cardCvcElement: ElementRef;

  stripe: any;
  cardNumber: any;
  cardExpiry: any;
  cardCvc: any;
  cardErrors: any;
  cardHandler = this.onChange.bind(this); // handler to show error
  loading = false;
  cardNumberValid = false;
  cardExpiryValid = false;
  cardCvcValid = false;

  constructor(private basketService: BasketService, private checkoutService: CheckoutService,
              private toastr: ToastrService, private router: Router) { }


  ngAfterViewInit(): void {
    this.stripe = Stripe('pk_test_51IMrBOJzRQfscld3IAZG36YKXqV60p9pSjfaMpjFGDBjUqfbPHs8c30pqPGt3Up43JsYe9DuNn0Pyg16grjZ9aFF00swMbM6sp');
    const elements = this.stripe.elements();

    this.cardNumber = elements.create('cardNumber'); // stripe element functionality
    this.cardNumber.mount(this.cardNumberElement.nativeElement);
    this.cardNumber.addEventListener('change', this.cardHandler);

    this.cardExpiry = elements.create('cardExpiry'); // stripe element functionality
    this.cardExpiry.mount(this.cardExpiryElement.nativeElement);
    this.cardExpiry.addEventListener('change', this.cardHandler);

    this.cardCvc = elements.create('cardCvc'); // stripe element functionality
    this.cardCvc.mount(this.cardCvcElement.nativeElement);
    this.cardCvc.addEventListener('change', this.cardHandler);
  }

  ngOnDestroy(): void {
    this.cardNumber.destroy();
    this.cardExpiry.destroy();
    this.cardCvc.destroy();
  }

  // syntax here is called destructuring.
  // onChange({error}){
  // if (error){
  //   this.cardErrors = error.message;
  // } else
  // {
  //   this.cardErrors = null;
  // }
  // }

  onChange(event){
   // console.log(event);
    if (event.error){
      this.cardErrors = event.error.message;
    } else
    {
      this.cardErrors = null;
    }

    switch (event.elementType) {
      case 'cardNumber':
        this.cardNumberValid = event.complete;
        break;
      case 'cardExpiry':
        this.cardExpiryValid = event.complete;
        break;
      case 'cardCvc':
        this.cardCvcValid = event.complete;
        break;
    }
    }


  // old way of submit order.
  // submitOrder(){
  //   const basket = this.basketService.getCurrentBasketValue();
  //   const orderToCreate = this.OrderToCreate(basket);
  //   this.checkoutService.createOrder(orderToCreate).subscribe((order: IOrder) => {
  //     //this.toastr.success('Order created successfully');
  //     this.stripe.confirmCardPayment(basket.clientSecret, {
  //       payment_method: {
  //         card: this.cardNumber,
  //         billing_details: {
  //           name: this.checkoutForm.get('paymentForm').get('nameOnCard').value
  //         }
  //       }
  //     }).then(result => {
  //       console.log(result);
  //       if (result.paymentIntent){
  //           this.basketService.deleteLocalBasket(basket.id);
  //           const navigationExtras: NavigationExtras = {state: order};
  //           this.router.navigate(['checkout/success'], navigationExtras);
  //       } else {
  //         this.toastr.error(result.error.message);
  //       }

  //     });

  //   }, error => {
  //     this.toastr.error(error.message);
  //     console.log(error);
  //   });
  // }

  async submitOrder() {
    this.loading = true;
    const basket = this.basketService.getCurrentBasketValue();
    try {
      const createdOrder = await this.createOrder(basket);
      const paymentResult = await this.confirmPaymentWithStripe(basket);

      if (paymentResult.paymentIntent){
      this.basketService.deleteBasket(basket);
      const navigationExtras: NavigationExtras = {state: createdOrder};
      this.router.navigate(['checkout/success'], navigationExtras);
      } else {
        this.toastr.error(paymentResult.error.message);
      }
      this.loading = false;
    }
    catch (error) {
      console.log(error);
      this.loading = false;
    }
  }

  private async confirmPaymentWithStripe(basket: IBasket) {
   return this.stripe.confirmCardPayment(basket.clientSecret, {
      payment_method: {
        card: this.cardNumber,
        billing_details: {
          name: this.checkoutForm.get('paymentForm').get('nameOnCard').value
        }
      }
    });
  }

  private async createOrder(basket: IBasket): Promise<object> {
    const orderToCreate = this.OrderToCreate(basket);
    return this.checkoutService.createOrder(orderToCreate).toPromise();
  }

  private OrderToCreate(basket: IBasket) {
    return {
      basketId: basket.id,
      deliveryMethodId: +this.checkoutForm.get('deliveryForm').get('deliveryMethod').value,
      shipToAddress: this.checkoutForm.get('addressForm').value
    };
  }

}
