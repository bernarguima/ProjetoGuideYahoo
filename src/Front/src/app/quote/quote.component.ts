import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';  
import { Observable } from 'rxjs';  
import { QuoteService } from '../quote.service';  
import { Quote } from '../quote';  


@Component({
  selector: 'app-quote',
  templateUrl: './quote.component.html',
  styleUrls: ['./quote.component.css']
})
export class QuoteComponent implements OnInit {
 
  allQuotes: Observable<Quote[]>;  

  constructor( private quoteService:QuoteService) { }

  ngOnInit() {

    this.loadAllQuotes();  
  }

  loadAllQuotes() {  
    this.allQuotes = this.quoteService.getAllQuotes();  
  } 
  

}
