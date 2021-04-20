import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Quote } from './quote'; 




@Injectable({
  providedIn: 'root'
})
export class QuoteService {

  url = 'https://localhost:44344/api/GetQuoteServiceCsv';  

  constructor(private http: HttpClient) { }

  getAllQuotes(): Observable<Quote[]> { 
    const myObservable = this.http.get<Quote[]>(this.url);
    
    return this.http.get<Quote[]>(this.url);
  }  

  error(error: HttpErrorResponse) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      errorMessage = error.error.message;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.log(errorMessage);
    return throwError(errorMessage);
  }
}

