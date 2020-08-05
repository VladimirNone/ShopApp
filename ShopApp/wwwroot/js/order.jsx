
function ProductFromOrderItem(props) {
    return (
        <div className="product_order_item">
            <a className="product_order_item_name">{props.name}</a>
            <div className="product_order_item_count">Количество: {props.count}</div>
            <div className="product_order_item_price">Цена: {props.price}</div>
        </div>
    );
}

class Order extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            order: {},
            orderedProducts: []
        };
        $.get(window.location.origin + "/api" + window.location.pathname, resp => this.setState({ order: resp, orderedProducts: resp.orderedProducts }));
    }

    render() {

        return (
            <div className="order_wrapper">
                <div className="order_header">
                    <h2>Заказ номер {this.state.order.id}</h2>
                </div>
                <div className="order_details">

                </div>
                <div className="order_body">
                    {this.state.orderedProducts.map(value => <ProductFromOrderItem price={value.product.price} name={value.product.name} count={value.count} key={value.id}/>)}
                </div>
            </div>
        );
    }
}

ReactDOM.render(<Order />, document.getElementById("content"));

ReactDOM.render(<Seacher />, document.getElementById("seacher"));
ReactDOM.render(<Categories />, document.getElementById("listOfCategories"));
ReactDOM.render(<HandlerUserBlock />, document.getElementById("authorization"));