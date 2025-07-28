import { TestBed } from '@angular/core/testing';
import { StoryService } from './story.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('StoryService', () => {
  let service: StoryService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ]
    });
    service = TestBed.inject(StoryService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return stories', () => {
    const mockResult = {
      items: [
        {
          title: 'Title 1',
          text: 'This is some text',
          url: 'www.story1.com'
        },
        {
          title: 'Title 2',
          text: 'This is some text for story 2',
          url: 'www.story2.com'
        },
      ],
      pageSize: 10,
      pageNumber: 1,
      totalCount: 2
    };

    service.getStories(1, 10).subscribe(result => {
      console.log(result);
      expect(result).toBeTruthy;
      expect(result.items).toBeTruthy();
      expect(result.items.length).toEqual(2);
      expect(result.pageNumber).toEqual(1);
      expect(result.pageSize).toEqual(10);
      expect(result.totalCount).toEqual(2);
    });

    const req = httpMock.expectOne('/story?pageNumber=1&pageSize=10');
    expect(req.request.method).toBe('GET');
    req.flush(mockResult);
  });
});
