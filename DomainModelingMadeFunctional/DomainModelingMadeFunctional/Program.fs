// And types are product types
// OR types are sum types

// OR Types - Like Enum
type AppleVariety = 
    | Fuji 
    | Honeycrisp 
    | Gala 
    | GrannySmith

type BannanaVariety = 
    | Cavendish 
    | GrosMichel
    | Plantain

type CherryVariety = 
    | Bing 
    | Rainier

// Record - AND type

type FruitSalad =
    {
        Apple: AppleVariety
        Bannana: BannanaVariety
        Cherry: CherryVariety
    }

// Discriminated Union - OR type (Choice type)

type FruitSnack =
   | Apple of AppleVariety
   | Bannana of BannanaVariety
   | Cherry of CherryVariety

let bannanaSnack = Bannana Cavendish