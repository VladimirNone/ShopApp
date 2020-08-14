
function UserProduct(props) {
    let zebra = props.index % 2 == 1 ? " zebra" : "";

    return (
        <div className={"product_order_item " + zebra}>
            <div className="product_order_item_info">
                <a className="product_order_item_name">{props.item.product.name}</a>
                <div className="product_order_item_count">Количество: {props.item.count}</div>
                <div className="product_order_item_price">Цена: {props.item.product.price}</div>
            </div>
            <div className="product_order_item_interaction">
                <button className="product_order_item_interaction_cancel" onClick={() => props.cancel(props.item.id)}> Отмена </button>
            </div>
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

        this.handleCancellation = this.handleCancellation.bind(this);
        this.generalPrice = this.generalPrice.bind(this);
    }

    handleCancellation(id) {
        $.post(window.location.origin + "/api/orderedProduct/cancel", { orderedProductId: id, userId: this.state.order.customerId }, resp => {
            this.state.orderedProducts.splice(this.state.orderedProducts.findIndex(v => id == v.id), 1)
            this.forceUpdate();
        });
    }

    generalPrice(listOfProds) {
        let sum = 0;
        for (let i = 0; i < listOfProds.length; i++) {
            sum += listOfProds[i].count * listOfProds[i].product.price;
        }
        return sum.toFixed(2);
    }

    render() {

        return (
            <div className="order_wrapper">
                <div className="order_header">
                    <h2>Заказ номер {this.state.order.id}</h2>
                    <h2>Общая стоимость: {this.generalPrice(this.state.orderedProducts)}</h2>
                </div>
                <div className="order_details">

                </div>
                <div className="order_body">
                    {this.state.orderedProducts.map((value, i) => <UserProduct item={value} index={i} id={value.id} key={value.id} cancel={this.handleCancellation} />)}
                </div>
            </div>
        );
    }
}

ReactDOM.render(<Order />, document.getElementById("content"));

ReactDOM.render(<Seacher />, document.getElementById("seacher"));
ReactDOM.render(<Categories />, document.getElementById("listOfCategories"));
ReactDOM.render(<HandlerUserBlock />, document.getElementById("authorization"));