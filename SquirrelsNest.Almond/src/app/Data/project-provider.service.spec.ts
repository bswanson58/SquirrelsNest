import { TestBed } from '@angular/core/testing';

import { ProjectProviderService } from './project-provider.service';

describe('ProjectProviderService', () => {
  let service: ProjectProviderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProjectProviderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
