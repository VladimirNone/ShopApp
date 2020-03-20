
let urlOfDomen = "api";



function Grid_item(props) {
    return (<li className={"grid__item" + " " + props.additionalClass}>
        <a href="https://www.google.com/?gws_rd=ssl">
            <img src={props.image}/>
            <p className="product_name">{props.productName}</p>
            <p className="product_price">{props.productPrice}</p>
        </a>
    </li>);

}

function Grid_row(props) {
    return (<li className="grid__row">
        <ul> {props.items.map((item, i) =>
            <Grid_item productName={item.name}
                image={item.linkToImage}
                productPrice={item.price}
                additionalClass={item.additionalClass === undefined ? "" : item.additionalClass}
                key={item.id} />)}
        </ul>
    </li>);
}

class Grid extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            countItemsInRow: 4,
            data: []
        };

        this.foo = this.foo.bind(this);
    }

    componentWillMount() {
/*        $.get(urlOfDomen + "prods", responseData => {
            this.setState({
                data: responseData
            })
        });*/

        let xhr = new XMLHttpRequest();
        xhr.open("get", urlOfDomen +"/"+ "prods", false);
        xhr.onload = function () {
            let responseData = JSON.parse(xhr.responseText);
            this.setState({ data: responseData });
        }.bind(this);
        xhr.send();

    }

    foo() {
        let arr = [];
        let length = this.state.data.length;
        let count = this.state.countItemsInRow;

        for (let i = count; ; i += count) {
            if (i <= length) {
                let buffer = [];
                for (let j = 0; j < i; j++)
                    buffer.push(this.state.data[i - j - 1]);
                arr.push((<Grid_row key={i * 1000} items={buffer} />));
            }
            else {
                let buffer = [];

                for (let j = 0; j < length - (i - count); j++)
                    buffer.push(this.state.data[(i - count + 1) + j - 1]);

                for (let j = 0; j < count - (length - (i - count)); j++)
                    buffer.push({ linkToImage: "", name: "", price: "", id: -(j + 1), additionalClass: "invisible__product" });

                arr.push((<Grid_row key={i * 1000} items={buffer} />));
                break;
            }
        }
        return arr;
    }

    render() {
        return this.foo();
    }
}

ReactDOM.render(<Grid />, document.getElementById("content"));