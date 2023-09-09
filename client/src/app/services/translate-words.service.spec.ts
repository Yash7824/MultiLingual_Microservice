import { TestBed } from '@angular/core/testing';

import { TranslateWordsService } from './translate-words.service';

describe('TranslateWordsService', () => {
  let service: TranslateWordsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TranslateWordsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
