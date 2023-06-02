import { TestBed } from '@angular/core/testing';

import { ReceitaServiceService } from './receita-service.service';

describe('ReceitaServiceService', () => {
  let service: ReceitaServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReceitaServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
