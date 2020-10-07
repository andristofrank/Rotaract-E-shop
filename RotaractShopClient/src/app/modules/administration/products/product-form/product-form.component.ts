import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {ResourcesService} from '../../../../services/resources.service';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {Product} from '../../../../../../APIs/models/Product';

@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.css']
})
export class ProductFormComponent implements OnInit {
  submitted;
  productForm: FormGroup;
  image: string;
  header;
  @Input() product: Product;
  @Output() updatedProduct = new EventEmitter<any>();
  constructor(private formBuilder: FormBuilder,
              private resourcesService: ResourcesService,
              private modalService: NgbModal) {
  }
  ngOnInit(): void {
    if (this.product) {
      this.header = 'Update product: ' + this.product.Name;
      this.productForm = this.formBuilder.group({
        name: new FormControl(this.product.Name,
          {validators: Validators.required}),
        description: new FormControl(this.product.Description),
        price: new FormControl(this.product.Price,
          {validators: [Validators.required, Validators.min(1)]})
      });
      this.image = this.product.ImageRef;
    } else {
      this.header = 'Add new product';
      this.productForm = this.formBuilder.group({
        name: new FormControl(null,
          {validators: Validators.required}),
        description: new FormControl(null),
        price: new FormControl(null,
          {validators: [Validators.required, Validators.min(1)]})
      });
      this.image = null;
    }
  }
  onSubmit() {
    this.submitted = true;
    // stop if form is invalid
    if (this.productForm.invalid) {
      return;
    }
    const formProduct = this.productForm.getRawValue();
    const product: Product = {
      Name: formProduct.name,
      Description: formProduct.description,
      ImageRef: this.image,
      Price: formProduct.price.toFixed(2)
    };
    if (this.product) {
      const product: Product = {
        ProductId: this.product.ProductId,
        Name: formProduct.name,
        Description: formProduct.description,
        ImageRef: this.image,
        Price: formProduct.price.toFixed(2)
      };
      this.resourcesService.updateProduct(product).subscribe(
        (res) => {
          this.updatedProduct.emit(res);
          this.modalService.dismissAll(); },
        (err) => { alert(err); }
      );
    } else {
      this.resourcesService.addProduct(product).subscribe(
        (res) => {
          this.updatedProduct.emit(res);
          this.modalService.dismissAll(); },
        (err) => { alert(err); }
      );
    }
  }
  closeModal() {
    this.modalService.dismissAll();
  }
  getImage(image){
    this.image = image;
  }
  get f() {
    return this.productForm.controls;
  }
}
