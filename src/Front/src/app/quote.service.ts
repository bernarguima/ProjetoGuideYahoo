import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';  
import { HttpHeaders } from '@angular/common/http';  
import { Observable, throwError } from 'rxjs';  
import { Quote } from './quote'; 
import { catchError, map } from "rxjs/operators";

var httpOptions = {headers: new HttpHeaders({"Content-Type": "application/json"})};

@Injectable({
  providedIn: 'root'
})
export class QuoteService {

  url = 'https://localhost:44344/api/GetQuoteServiceCsv';  

  constructor(private http: HttpClient) { }

  getAllQuotes(): Observable<Quote[]> { 
    const myObservable = this.http.get<Quote[]>(this.url).pipe(catchError(this.serviceError));

    const myObserver = {
      next: x => console.log('Observer got a next value: ' + x),
      error: err => console.error('Observer got an error: ' + err),
      complete: () => console.log('Observer got a complete notification'),
    };
    
    return myObservable;
  }  

  protected serviceError(error: Response | any) {
    let errMsg: string;

    if (error instanceof Response) {

        errMsg = `${error.status} - ${error.statusText || ''}`;
    }
    else {
        errMsg = error.message ? error.message : error.toString();
    }

    console.error(errMsg);
    return throwError(errMsg);
}
}
