/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ModeratorService } from './moderator.service';

describe('Service: Moderator', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ModeratorService]
    });
  });

  it('should ...', inject([ModeratorService], (service: ModeratorService) => {
    expect(service).toBeTruthy();
  }));
});
