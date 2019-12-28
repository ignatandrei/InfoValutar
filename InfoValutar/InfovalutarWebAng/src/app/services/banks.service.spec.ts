import { TestBed } from '@angular/core/testing';

import { BanksService } from './banks.service';

describe('BanksService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BanksService = TestBed.get(BanksService);
    expect(service).toBeTruthy();
  });
});
