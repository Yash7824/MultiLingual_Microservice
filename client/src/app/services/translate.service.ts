import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environments } from 'src/environments/environement';
import { TranslateModel } from '../models/TranslateModel';

@Injectable({
  providedIn: 'root'
})
export class TranslateService {

  httpOption: Object = {
    headers: new HttpHeaders({ 'Access-Control-Allow-Origin': '*'}),
    responseType: 'text'
 }
 
  constructor(private http: HttpClient) { }

  public getTranslateText(input: string, code: string): Observable<string>{
    return this.http.get<string>(`${environments.apiUrl}/Translation/translate?text=${input}&code=${code}`, this.httpOption);
  }

  public posttranslateTextArray(inputArray: string[], code: string): Observable<string[]>{
    return this.http.post<string[]>(`${environments.apiUrl}/Translation/translateArray?code=${code}`, inputArray)
  }

  public postTranslateObject(translateModel: TranslateModel, code: string){
    return this.http.post(`${environments.apiUrl}/Translation/TranslateObject?code=${code}`, translateModel)
  }

  public postTranslateCustomization(code: string, inputArray: string[]) : Observable<string[]>{
    return this.http.post<string[]>(`${environments.apiUrl}/Translation/translationNewCustomization/${code}`, inputArray)
  }
}

