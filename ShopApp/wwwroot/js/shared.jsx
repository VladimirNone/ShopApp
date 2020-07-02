let host = "https://localhost:5001/";

let urlOfDomen = "api";

let lastRequest = "";


function LoadData(pattern, data, relatedWithProds) {
    let something = window.location.pathname;
    let localPattern;
    if (relatedWithProds)
        localPattern = window.location.origin + "/api" + something + (something.localeCompare("/") == 0 ? "prods/" : "/") + pattern;
    else
        localPattern = window.location.origin + "/api" + pattern;

    if (localPattern === lastRequest)
        return data;

    //$.get(localPattern, responseData => data = responseData);

    let xhr = new XMLHttpRequest();
    xhr.open("get", localPattern, false);
    xhr.onload = () => data = JSON.parse(xhr.responseText);
    try {
        xhr.send();
    }
    catch (err) {

    }

    lastRequest = localPattern;
    return data;
}

function Grid_item(props) {
    return (<li className={"grid__item" + " " + props.additionalClass}>
        <a href={"product/" + props.id}>
            <img src={props.image} />
            <div className="item__info">
                <p className="item_name">{props.productName}</p>
                <p className="item_price">{props.productPrice}</p>
            </div>
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
                key={item.id}
                id={item.id} />)}
        </ul>
    </li>);
};

class Grid extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            countItemsInRow: 4,
            data: [],
            countQuery: 0,
            queryChanged: false,
        };

        this.generateProductList = this.generateProductList.bind(this);
        this.autoLoadProducts = this.autoLoadProducts.bind(this);
    }

    autoLoadProducts() {
        //Текущее расположение пользователя с учетом прокрутки
        let scrollTop = window.pageYOffset || document.documentElement.scrollTop;
        //Высота с учетом прокрутки
        let scrollHeight = Math.max(
            document.body.scrollHeight, document.documentElement.scrollHeight,
            document.body.offsetHeight, document.documentElement.offsetHeight,
            document.body.clientHeight, document.documentElement.clientHeight
        );
        //Высота окна браузера
        let windowHeight = window.innerHeight;

        if (scrollTop + windowHeight > scrollHeight - windowHeight / 2 || this.state.queryChanged) {

            let newData = LoadData(this.state.countQuery, [], true);
            if (newData.length === 0)
                return;
            if (!this.state.queryChanged)
                newData = this.state.data.concat(newData);

            this.setState({
                data: newData,
                countQuery: this.state.countQuery + 1,
                queryChanged: false,
            })
        }
    }

    componentWillReceiveProps(nextProps) {
        this.setState({
            countQuery: 0,
            queryChanged: true
        });
    }

    componentWillMount() {
        if (this.props.data != undefined)
            this.setState({ data: this.props.data });
    }

    componentDidMount() {
        this.autoLoadProducts()
        window.setInterval(
            () => this.autoLoadProducts(),
            2000,
        );
    }

    generateProductList() {
        let arr = [];
        let length = this.state.data.length;
        let count = this.state.countItemsInRow;

        for (let i = count; ; i += count) {
            if (i <= length) {
                let buffer = [];
                for (let j = i - count; j < i; j++)
                    buffer.push(this.state.data[j]);
                arr.push((<Grid_row key={i * 1000} items={buffer} />));
            }
            else {
                if (length - (i - count) == 0)
                    break;
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
        return (<ul className="grid">{this.generateProductList()}</ul>);
    }
}

function Category(props) {
    return (<li><a onClick={() => props.click(props.value)}>{props.value}</a></li>);
}

class Categories extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: [],
        };

        this.handleClick = this.handleClick.bind(this);
    }
    componentDidMount() {
        this.setState({ data: LoadData("/categories", [], false) })
    }

    handleClick(typeOfProduct) {
        window.location.href = host + 'category/' + typeOfProduct;
        ReactDOM.render(<Grid />, document.getElementById("content"));
        //query={"category/" + typeOfProduct}
    }

    render() {
        return (this.state.data.map((item, i) => <Category key={item.id} value={item.nameOfType} click={this.handleClick} />));
    }
}

class Seacher extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            searchedThing: "",
        }

        this.onChange = this.onChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit(e) {
        e.preventDefault();
        if (!this.state.searchedThing)
            return;
        window.location.href = host + 'search/'+this.state.searchedThing;
        ReactDOM.render(<Grid  />, document.getElementById("content"));
        //query={"search/" + this.state.searchedThing}
        this.setState({ searchedThing: "" });
    }

    onChange(e) {
        this.setState({
            searchedThing: e.target.value,
        });
    }

    render() {
        return (<form onSubmit={this.handleSubmit}>
            <input type="text" name="searching_field" placeholder="Поиск" onChange={this.onChange} />
            <button type="submit" formMethod="post">Найти</button>
        </form>);
    }
}
