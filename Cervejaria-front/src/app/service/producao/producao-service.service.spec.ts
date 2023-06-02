import { TestBed } from '@angular/core/testing';

import { ProducaoServiceService } from './producao-service.service';

describe('ProducaoServiceService', () => {
  let service: ProducaoServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProducaoServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
