import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {NgxImageCompressService} from 'ngx-image-compress';
@Component({
  selector: 'app-image-loader',
  templateUrl: './image-loader.component.html',
  styleUrls: ['./image-loader.component.css']
})
export class ImageLoaderComponent implements OnInit {
isImageSaved: boolean;
ImageBased64: string;
@Input() existingImage: string
@Output() image64 = new EventEmitter<string>();
  constructor( private imageCompressService: NgxImageCompressService) { }

  ngOnInit(): void {
   if (this.existingImage) {
     this.isImageSaved = true;
     this.ImageBased64 = this.existingImage;
   }
  }
  imageUpload() {
    this.imageCompressService.uploadFile().then(({image, orientation}) =>{
      this.imageCompressService.compressFile(image, orientation, 50, 50).then(
        result => {
          this.ImageBased64 = result;
          this.isImageSaved = true;
          this.image64.emit(this.ImageBased64);
        }
      )
    });
  }
  removeImage() {
    this.ImageBased64 = null;
    this.isImageSaved = false;
  }

}
