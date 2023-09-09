import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { allWordsObj } from 'src/assets/data/words';
import { allWordsList } from 'src/assets/data/words';
import { allWordsListStatic } from 'src/assets/data/words';

@Injectable({
  providedIn: 'root'
})
export class TranslateWordsService {

  constructor(private http: HttpClient) {}

  AllWordsObj = allWordsObj
  AllWordsList = allWordsList
  AllWordsListStatic = allWordsListStatic

  
 
}


