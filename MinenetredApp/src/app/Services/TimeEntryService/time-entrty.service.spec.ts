import { TestBed } from '@angular/core/testing';

import { TimeEntrtyService } from './time-entrty.service';

describe('TimeEntrtyService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TimeEntrtyService = TestBed.get(TimeEntrtyService);
    expect(service).toBeTruthy();
  });
});
