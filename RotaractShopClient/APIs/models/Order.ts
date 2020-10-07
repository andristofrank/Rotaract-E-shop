import {ProductItem} from "./ProductItem";
import {ShippingDetails} from "./ShippingDetails";
import {OrderItem} from "./OrderItem";

export class Order {
  OrderId ?: number;
  OrderItems ?: OrderItem[];
  ShippingDetailsModel: ShippingDetails;
  TotalPrice ?: number;
}
