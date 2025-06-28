import { TestBed } from '@angular/core/testing';

import { AlternativeService } from './alternative.service';

describe('AlternativeService', () => {
  let service: AlternativeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AlternativeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
