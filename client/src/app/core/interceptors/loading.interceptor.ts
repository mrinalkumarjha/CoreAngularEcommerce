import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BusyService } from '../services/busy.service';
import { Injectable } from '@angular/core';
import { delay, finalize } from 'rxjs/operators';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
    constructor(private busyService: BusyService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        if(req.method === 'POST' && req.url.includes('orders')) {
            // here we are skipping loader for post order
            return next.handle(req);
        }

        if(req.method === 'DELETE') {
            return next.handle(req);
        }

        if (req.url.includes('emailexists')){
            // we are executing spinner only if request is not for email check
            //this.busyService.busy();
            return next.handle(req);
        }
        this.busyService.busy();
        return next.handle(req)
        .pipe(
            // delay(5),

            finalize(() => {
                        this.busyService.idle();
                    })

        );

        // if (req.method === 'POST' && req.url.includes('orders')) {
        //     return next.handle(req);
        // }
        // if (req.method === 'DELETE') {
        //     return next.handle(req);
        // }
        // if (req.url.includes('emailexists')) {
        //     return next.handle(req);
        // }
        // this.busyService.busy();
        // return next.handle(req).pipe(
        //     finalize(() => {
        //         this.busyService.idle();
        //     })
        // );
    }
}