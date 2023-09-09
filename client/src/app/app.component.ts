import { Component } from '@angular/core';
import { TranslateService } from './services/translate.service';
import { forkJoin } from 'rxjs';
import { TranslateWordsService } from './services/translate-words.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Multilingual';
  children: any;

  constructor(private translateService: TranslateService,
    private translateWordSrv: TranslateWordsService) { }

  step: number = 0;

  clickTab(stepValue: number){
    this.step = stepValue;
    // localStorage.setItem('step', JSON.stringify(this.step));
  }

  TextObj = this.translateWordSrv.AllWordsObj
  TextObjArray = this.translateWordSrv.AllWordsList;
  textArrayStatic = this.translateWordSrv.AllWordsListStatic

  english: string = 'en'
  hindi: string = 'hi'
  marathi: string = 'mr'
  tamil: string = 'ta'
  telugu: string = 'te'
  gujarati: string = 'gu'

  selectedOption: string = '';

  Translate(input: string, code: string) {

    this.translateService.getTranslateText(input, code).subscribe({
      next: (response) => {
        this.TextObjArray[0] = response;
      }
    })
  }


  TranslateAll(inputArray: string[], code: string) {

    this.translateService.postTranslateCustomization(code, this.textArrayStatic).subscribe({
      next: (response) => {
        for (let i = 0; i < this.TextObjArray.length; i++) {
          this.TextObjArray[i] = response[i]
        }
      }
    })


  }

  onChangeHandler(event: any) {
    this.selectedOption = event.target.value

    this.TranslateAll(this.TextObjArray, this.selectedOption);

  }

  

}
