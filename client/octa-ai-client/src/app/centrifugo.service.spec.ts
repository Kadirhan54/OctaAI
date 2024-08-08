import { TestBed } from '@angular/core/testing';

import { CentrifugoService } from './centrifugo.service';

describe('CentrifugoService', () => {
  let service: CentrifugoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CentrifugoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
