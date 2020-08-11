
function ProductFromBasketItem(props) {
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

class Basket extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            basket: {},
            selectedProducts: []
        };
        $.get(window.location.origin + "/api/basket", resp => this.setState({ basket: resp, selectedProducts: resp.orderedProducts }));

        this.handleCancellation = this.handleCancellation.bind(this);
        this.generalPrice = this.generalPrice.bind(this);
        this.placeAnOrder = this.placeAnOrder.bind(this);
    }

    handleCancellation(id) {
        $.post(window.location.origin + "/api/basket/cancel/", { selectedProductId: id }, resp => {
            this.state.selectedProducts.splice(this.state.selectedProducts.findIndex(v => id == v.id), 1)
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


    placeAnOrder() {
        $.post(window.location.origin + "/api/placeAnOrder");
    }

    render() {
        return (
            <div className="order_wrapper">
                <div className="order_header">
                    <h2>Корзина</h2>
                    <h2>Общая стоимость: {this.generalPrice(this.state.selectedProducts)}</h2>
                </div>
                <div className="order_details">

                </div>
                <div className="order_body">
                    {(this.state.selectedProducts.length == 0)
                        ? (<h3>Корзина пуста</h3>)
                        : this.state.selectedProducts.map((value, i) => <ProductFromBasketItem item={value} index={i} id={value.id} key={value.id} cancel={this.handleCancellation} />)}
                </div>
                <div className="order_button">
                    {(this.state.selectedProducts.length == 0)
                        ? (<div/>)
                        : (<button onClick={this.placeAnOrder}>Оформить</button>)}
                </div>
            </div>
        );
    }
}

ReactDOM.render(<Basket />, document.getElementById("content"));

ReactDOM.render(<Seacher />, document.getElementById("seacher"));
ReactDOM.render(<Categories />, document.getElementById("listOfCategories"));
ReactDOM.render(<HandlerUserBlock />, document.getElementById("authorization"));